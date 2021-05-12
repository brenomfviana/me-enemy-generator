import os
import sys
import random
import numpy as np

# --- Remember

# The MAP-Elites matrix is composed of 18 Elites


# --- Initialization

# Initialize the random seed
random.seed(0)

# Define the number of executions by set of parameters
executions = 10


# --- Define the set of parameters

# Set of numbers of generations
generations = [50, 100, 200]
# Set of numbers of individuals of the initial populations
initial_pops = [5, 10, 15]
# Set of numbers of individuals of offspring
offspring = [1, 2, 5, 10]

# generations = [50]
# initial_pops = [5]
# offspring = [1]

# --- Perform experiment

# Compile code
os.system('dotnet publish')

# Location of the executable
executable = './bin/Debug/net5.0/publish/OverlordEnemyGenerator '

# Run algorithm for all combination of sets of parameters
for gs in generations:
    for ip in initial_pops:
        for of in offspring:
            for ex in range(executions):
                random_seed = random.randint(0, np.iinfo(np.int32).max - 1)
                parameters = str(ex) + ' ' + str(random_seed) + ' ' \
                    + str(gs) + ' ' + str(ip) + ' ' + str(of)
                print(parameters)
                os.system(executable + parameters)
                os.system('python plot.py ' + parameters)