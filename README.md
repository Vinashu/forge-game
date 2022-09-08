# FORGE Game

## Introduction
FORGE Game is an integral part of the FORGE framework. It was created to demonstrate how to integrate the FORGE framework into games created with Unity3D.

To know more about the FORGE framework, visit its [repository](https://github.com/Vinashu/forge-server). A website application is also created to demonstrate the integration with the FORGE framework, and its repository is available [here](https://github.com/Vinashu/forge-app).

## Gameplay
The game is simple: the goal is to guess the result of a coin flip. After making a guess, the drawn coin face is highlighted in green, while the other face is highlighted in red. The player receives points and badges according to their performance in the game.

## Integration
After each bet, the game sends an HTTP request to the FORGE framework containing an array of messages. In the context of FORGE, a message is an object with a variable name and value. FORGE evaluates each of the messages and returns rewards to the game. The game evaluates the rewards received and updates the game interface.

## Live Demo
It is possible to test the FORGE Game directly online. The FORGE is hosted on a free service that goes offline after 10 minutes of inactivity. The server works again as soon as a request is made to it.

The Forge Game: https://vinashu.github.io/forge-game/

The Forge App: https://forge-app-example.herokuapp.com/

There is also an online version of the FORGE framework available for testing. All rewards created in it will be visible in the FORGE Game and in the FORGE App.

The FORGE Framework: https://forge-server-example.herokuapp.com/
