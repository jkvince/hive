import torch
import torch.nn as nn
import torch.optim as optim
import numpy as np

import sys

# 28 total pieces, meaning the biggest the board could be is 28 pieces long.
# 28 x 28 x (7) = 5488
#
#
#
#

# ==========================================
# 1. THE NEURAL NETWORK (The Brain)
# ==========================================
class HiveNet(nn.Module):
    def __init__(self, input_size):
        super(HiveNet, self).__init__()
        # Simple architecture: Input -> Hidden -> Output
        self.layer1 = nn.Linear(input_size, 128)
        self.relu = nn.ReLU()

        self.layer2 = nn.Linear(128, 64)
        self.layer3 = nn.Linear(64, 64)

        self.output = nn.Linear(64, 1) # Single output: Score (-1 to 1)
        self.tanh = nn.Tanh()          # Forces output to be between -1 and 1

    def forward(self, x):
        x = self.layer1(x)
        x = self.relu(x)
        x = self.layer2(x)
        x = self.layer3(x)
        x = self.relu(x)
        x = self.output(x)
        return self.tanh(x)

# ==========================================
# 2. HELPER: CALCULATE DISCOUNTED REWARDS
# ==========================================
def calculate_targets(game_history, final_result, gamma=0.99):
    """
    This solves the 'Credit Assignment' problem.
    It takes the final result (+1 or -1) and spreads it back through time.
    
    Turn 30 (End) = 1.0
    Turn 29       = 0.99
    Turn 28       = 0.98
    ...
    Turn 1        = 0.74
    """
    targets = []
    
    # We iterate backwards from the last move to the first
    # Because the "reward" propagates from the end of the game
    current_val = final_result
    
    # Create a list of the same length as history
    for _ in range(len(game_history)):
        targets.insert(0, current_val) # Add to front of list
        current_val = current_val * gamma # Decay the value for the previous turn
        
    return targets

# ==========================================
# 3. MOCK DATA LOADER (Replace this with your JSON load)
# ==========================================
def load_data_from_godot():
    # In reality, you would do:
    # with open("game_history.json", "r") as f:
    #     data = json.load(f)
    
    print("Generating fake Godot game data...")
    
    # Simulating 100 games of Hive (input size 225 for a 15x15 board)
    training_data = []
    
    for game_id in range(100):
        # Random game length between 20 and 40 turns
        game_length = np.random.randint(20, 40)
        
        # Who won? (1 = White, -1 = Black)
        winner = 1 if np.random.random() > 0.5 else -1
        
        # Fake board states (Just random noise for this demo)
        game_history = [np.random.rand(225).tolist() for _ in range(game_length)]
        
        # --- CRITICAL STEP: Add the "Discounted Labels" ---
        targets = calculate_targets(game_history, winner, gamma=0.95)
        
        # Pair the board (input) with the calculated target (label)
        for board, target in zip(game_history, targets):
            training_data.append((board, target))
            
    return training_data

# ==========================================
# 4. THE TRAINING LOOP
# ==========================================
def train():
    # Hyperparameters
    INPUT_SIZE = 225 # Example: 15x15 board
    LEARNING_RATE = 0.001
    EPOCHS = 5
    
    # Initialize Model
    model = HiveNet(INPUT_SIZE)
    optimizer = optim.Adam(model.parameters(), lr=LEARNING_RATE)
    criterion = nn.MSELoss() # Mean Squared Error: (Guess - Target)^2
    
    # Load Data
    data = load_data_from_godot()
    print(f"Loaded {len(data)} individual positions/turns to train on.")
    
    # Convert to PyTorch Tensors
    boards = torch.tensor([item[0] for item in data], dtype=torch.float32)
    labels = torch.tensor([item[1] for item in data], dtype=torch.float32).view(-1, 1)

    # Loop
    print("\nStarting Training...")
    for epoch in range(EPOCHS):
        optimizer.zero_grad()   # Reset gradients
        outputs = model(boards) # Ask AI to guess scores for all boards
        loss = criterion(outputs, labels) # Compare guesses to "Discounted Targets"
        
        loss.backward() # Calculate the math (Backpropagation)
        optimizer.step() # Update weights
        
        print(f"Epoch {epoch+1}/{EPOCHS}, Loss: {loss.item():.6f}")

    print("\nTraining Complete.")
    
    # Export weights for Godot?
    # torch.save(model.state_dict(), "hive_model.pth")
    # OR export to ONNX
    # dummy_input = torch.randn(1, INPUT_SIZE)
    # torch.onnx.export(model, dummy_input, "hive_brain.onnx")
    print("Model ready for export.")

if __name__ == "__main__":

    # create new model
    if sys.argv[1] == "new":
    	name = sys.argv[2]
    	#new_model()
    	
    elif sys.argv[1] == "train":
    	name = sys.argv[2]
		# train
    	#train()
