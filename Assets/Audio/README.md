# Audio Setup Guide

This folder contains all audio files used in the game.

## Folder Structure
- **SFX**: Contains sound effects (sword swing, damage, death, etc.)
- **Music**: Contains background music tracks

## How to Set Up Audio Manager

1. In Unity Editor, go to the top menu: **Game > Setup > Create Audio Manager**
2. This will create an AudioManager object in your scene and a prefab in Assets/Prefabs

## Required Audio Files

Place your audio files in the appropriate folders and assign them in the AudioManager inspector:

### Required SFX:
- Sword swing sound (for player attacks)
- Player damage sound (when player takes damage)
- Enemy damage sound (when enemies take damage)
- Enemy death sound (when enemies die)

## Manual Setup

If you prefer to set up manually:
1. Create a new GameObject in your scene called "AudioManager"
2. Add the AudioManager component to it
3. Create two AudioSource components: one for music and one for sound effects
4. Assign your audio clips to the appropriate fields in the AudioManager component

## Implementation Details

The audio system is already integrated with:
- Player sword swings (PlayerMovement.cs)
- Player damage (PlayerHealth.cs)
- Enemy damage and death (EnemyHealth.cs)

No additional code changes are required for basic functionality. 