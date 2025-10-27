# Stardreamer - Space Shooter Game

A 2D space shooter game similar to Galaga and Space Invaders, built with Unity.

## Game Features

- **Player Ship**: Move with WASD or arrow keys, shoot with spacebar or left mouse button
- **Enemy AI**: Multiple movement patterns including straight down, zigzag, circular, and side-to-side
- **Wave System**: Progressive difficulty with increasing enemy count and complexity
- **Scoring System**: Points for destroying enemies, lives system
- **UI System**: Score display, lives counter, wave indicator, pause menu, game over screen

## Game Controls

- **WASD** or **Arrow Keys**: Move player ship
- **Spacebar** or **Left Mouse Button**: Shoot
- **Escape**: Pause/Resume game

## Gameplay

1. **Objective**: Destroy enemy ships while avoiding their attacks
2. **Lives**: Start with 3 lives, lose one when hit by enemy or enemy bullet
3. **Waves**: Each wave has more enemies with different movement patterns
4. **Scoring**: Earn points for destroying enemies
5. **Game Over**: When all lives are lost

## Customization

### Enemy Types
Create different enemy prefabs with:
- Different sprites/colors
- Different health values
- Different movement patterns
- Different score values

### Difficulty Scaling
Adjust in GameManager:
- `spawnRate`: How often enemies spawn
- `enemiesPerWave`: Number of enemies per wave
- `waveDelay`: Delay between waves

### Player Settings
Adjust in PlayerController:
- `moveSpeed`: Player movement speed
- `fireRate`: How fast player can shoot
- `boundaryPadding`: Distance from screen edge

## Future Enhancements

- Power-ups (rapid fire, shields, multi-shot)
- Boss enemies
- Particle effects for explosions
- Advanced high score system
- Multiple backgrounds
- Mobile touch controls
