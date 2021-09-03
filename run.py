import os
import platform
import random
import numpy as np


# --- Initialization

# Initialize the random seed
random.seed(0)

# Define the number of executions of each set of parameters
executions = range(1)


# --- Define the set of parameters

# Numbers of generations
generations = [200]

# Initial population sizes
populations = [25]

# Mutation rates
mutations = [30]

# Crossover rates
crossovers = [80]


# --- Perform experiment

def run(g, p, m, c):
  # Generate a random seed
  rs = random.randint(0, np.iinfo(np.int32).max - 1)
  # Build the parameters
  parameters = ""
  for i in [rs, g, p, m, c]:
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
if platform.system() == 'Linux':
  executable = './bin/Debug/net5.0/publish/EnemyGenerator '
elif platform.system() == 'Windows':
  executable = 'start bin\\Debug\\net5.0\\publish\\EnemyGenerator '
else:
  print('This script is not able to run in this OS.')
  exit()

# Variables for progress
total = len(generations) * len(populations) * len(mutations) * len(crossovers) * len(executions)
i = 1

# Run the algorithm for all sets of parameters
for g in generations:
  for p in populations:
    for m in mutations:
      for c in crossovers:
        for _ in executions:
          # Run execuble
          run(g, p, m, c)
          # Print progress
          print("%.2f" % ((i / total) * 100))
          i += 1