import os
import sys
import platform
import random
import numpy as np



# --- Initialization

# Initialize the random seed
seed = random.randrange(sys.maxsize)
random.seed(seed)
print("Seed:", seed)
# random.seed(0)

# Define the number of executions of each set of parameters
executions = range(100)



# --- Define the set of parameters

# Numbers of generations
generations = [1000] # [100, 500, 1000]

# Initial population sizes
populations = [35] # [25, 35, 42]

# Intermediate population sizes
intermediates = [50, 100] # [20, 50, 100]

# Mutation rates
mutations = [20] # [15, 20, 25]

# Mutation rates for genes
genemutations = [30] # [20, 30, 40]

# Competitors
competitors = [2] # [2, 3, 4]

# Difficulties
difficulties = [10, 18, 26] # [10, 14, 18, 22, 26]



# --- Perform experiment

# Choose the executable
if platform.system() == 'Linux':
  executable = './bin/Debug/net5.0/publish/EnemyGenerator '
elif platform.system() == 'Windows':
  executable = 'bin\\Debug\\net5.0\\publish\\EnemyGenerator '
else:
  print('This script is not able to run in this OS.')
  exit()


# Compile project
os.system('dotnet publish')


def run(ge, po, ip, mu, gm, co, di):
  # Generate a random seed
  rs = random.randint(0, np.iinfo(np.int32).max - 1)
  # Build the parameters
  parameters = ""
  for i in [rs, ge, po, ip, mu, gm, co, di]:
    parameters += str(i) + ' '
  # Print parameters
  print('Parameters=[', parameters, ']')
  # Run algoritm for the current set of parameters
  os.system(executable + parameters)

# Variables to control the experiment progress
total = len(generations) * \
  len(populations) * \
  len(intermediates) * \
  len(mutations) * \
  len(genemutations) * \
  len(competitors) * \
  len(difficulties) * \
  len(executions)
i = 1

# Run the algorithm for all sets of parameters
print('Running')
for ge in generations:
  for po in populations:
    for ip in intermediates:
      for mu in mutations:
        for gm in genemutations:
          for co in competitors:
            for di in difficulties:
              for e in executions:
                # Run execuble
                run(ge, po, ip, mu, gm, co, di)
                # Print progress
                print("%.2f" % ((i / total) * 100))
                i += 1


# --- Plot charts of the experiment results

def plot(ge, po, ip, mu, gm, co, di):
  parameters = ''
  for i in [ge, po, ip, mu, gm, co, di]:
    parameters += str(i) + ' '
  os.system('python plot.py ' + parameters)

# Variables to control the plotting progress
total = len(generations) * \
  len(populations) * \
  len(intermediates) * \
  len(mutations) * \
  len(genemutations) * \
  len(competitors) * \
  len(difficulties)
i = 1

# Plot charts for all sets of parameters
print('Plotting')
for ge in generations:
  for po in populations:
    for ip in intermediates:
      for mu in mutations:
        for gm in genemutations:
          for co in competitors:
            for di in difficulties:
              # Plot charts
              plot(ge, po, ip, mu, gm, co, di)
              # Print progress
              print("%.2f" % ((i / total) * 100))
              i += 1