# MAP-Elites Enemy Generator

This program is an Enemy Generator that evolves enemies via a MAP-Elites Genetic
Algorithm.
The output of this program is a set of enemies written in JSON files.

**ATTENTION:**
This program was designed for the game prototype of the Game Research Group of
Universidade de São Paulo (USP).
Therefore, the designed enemy representation and difficulty function may not
work for other games.

This program can perform two tasks: generate a set of enemies and calculate
the difficulty of an entered enemy.

To generate enemies, this program receives seven arguments:
- [Optional] the save separately flag (`-s`);
  * if the flag `-s` is entered,  then the enemies on the final population
    will be written separately, each one in a single JSON file.
- a random seed;
- the number of generations;
- the initial population size;
- the intermediate population size;
- the mutation chance;
- the mutation chance of a single gene;
- the number of tournament competitors, and;
- the aimed difficulty of enemies.

To calculate the difficulty of a single enemy, this program receives two
arguments:
- the difficulty calculation flag (`-d`);
- the JSON file path of the enemy.


## Author

- Breno M. F. Viana


## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
