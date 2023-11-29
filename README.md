# WaveFunctionCollapse

The Wave Function Collapse (WFC) algorithm, created by [Maxim Gumin](https://github.com/mxgmn), is a fascinating and 
innovative tool in the realm of procedural generation, often used for generating bitmaps that resemble a given pattern.
The algorithm gained significant attention for its ability to create visually appealing, intricate patterns and layouts,
making it popular in game development and digital art.

## This Implementation

This is a little experiment to see if I can implement the basics of WFC algorithm in C# and [MonoGame](https://github.com/MonoGame/MonoGame).

## Maxim Gumin's Original Implementation

### Core Concept

- **Inspiration from Quantum Mechanics**: The name "Wave Function Collapse" is inspired by quantum mechanics, where a wave function represents a state with many possibilities until it collapses to a single outcome. Similarly, in WFC, each cell in a grid has multiple possibilities (like tiles or pixels) which collapse to one definite state.
- **Input Model**: The algorithm starts with an input sample (like an image) and analyzes it to understand the local patterns and rules (e.g., how tiles or pixels relate to their neighbors).

### Process

1. **Grid Initialization**: A grid is initialized where each cell represents a part of the output and can be in multiple states initially.
2. **Pattern Analysis**: The algorithm analyzes the input to create a set of unique patterns (like 2x2 or 3x3 blocks in an image).
3. **Superposition and Entropy**: Initially, each cell in the grid is in a state of superposition, meaning it can become any of the observed patterns. Entropy is a measure of uncertainty; cells with more possible states have higher entropy.
4. **Observation**: The algorithm selects a cell with the lowest entropy (least uncertainty) and collapses its state, choosing one of the possible patterns based on weighted randomness.
5. **Propagation**: This step updates neighboring cells' states based on the collapsed state of the selected cell, ensuring that only compatible states remain possible in each cell. This is repeated, propagating the constraints throughout the grid.
6. **Iteration**: Steps 4 and 5 are repeated, progressively collapsing more cells and reducing the overall entropy of the system until all cells have a definite state.

### Applications

- **Bitmap Generation**: Creating complex images that follow a specific style or pattern.
- **Level Design**: In game development, for generating maps and levels that are both random and structured.
- **Architectural Layouts**: Designing building layouts or urban plans procedurally.

### Key Features

- **Versatility**: It can be used with any tileset and rules, making it highly adaptable.
- **Non-deterministic**: Each run can produce a different result, which is ideal for procedural generation.

### Challenges

- **Handling Over-Constrained Systems**: Situations where no valid state can be found for a cell can occur, requiring backtracking or re-initialization.
- **Performance**: The algorithm can be computationally intensive, especially for large grids or complex rule sets.

WFC is a powerful tool for procedural content generation, offering a unique blend of randomness and structure that can be tailored to a wide range of applications. Its ability to create complex, rule-based patterns from simple inputs makes it a valuable asset in various fields, from game development to digital art.