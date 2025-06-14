The `MoreIntelligenterGrid` implementation is 4x faster than the Turtle traversing grid pattern. That was good fun to work on.

## Complexity Analysis

The Turtle Method walks the grid at O(N*M*P*Q) where the grid is an NxM matrix, P is the number of directions to walk in each spot (8), and Q is the length of the needle.

The Matrix method walks the grid in O(N*M*I*J) where I is the number of directions checked (4), and J is the forward and reverse regex (2).

## What's missed in Complexity Analysis

The Matrix method runs linearly over the problem space, with minimal branching, which maximizes CPU cache line usage and decreases load on the branch predictor.
