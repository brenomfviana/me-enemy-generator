import os
import sys
import random
import numpy as np


# --- Initialization

# Initialize the random seed
random.seed(0)

# Define the number of executions of each set of parameters
executions = range(1)


# --- Define the set of parameters

# Set the numbers of generations
generations = [100]

# Set the numbers of individuals of the initial populations
populations = [10]

# Set the numbers of individuals of offspring
offsprings = [1]


# --- Perform experiment

def run(g, p, o, e):
  # Generate a random seed
  rs = random.randint(0, np.iinfo(np.int32).max - 1)
  # Build the parameters
  parameters = ""
  for i in [e, rs, g, p, o]:
    parameters += str(i) + ' '
  # Print parameters
  print(parameters)
  # Run algoritm for the current set of parameters
  os.system(executable + parameters)
  # Plot the charts for the current set of parameters
  os.system('python plot.py ' + parameters)

# Compile the code
os.system('dotnet publish')

# Executable location
executable = './bin/Debug/net5.0/publish/OverlordEnemyGenerator '

# Variables for progress
total = len(generations) * len(populations) * len(offsprings) * len(executions)
i = 1

# Run the algorithm for all sets of parameters
for g in generations:
  for p in populations:
    for o in offsprings:
      for e in executions:
        # Run execuble
        run(g, p, o, e)
        # Print progress
        print("%.2f" % ((i / total) * 100))
        i += 1