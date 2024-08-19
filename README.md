# Proportional Navigation Demo in 3D
This demo is designed so users can create their own engagement setups by modifying each object's starting conditions and maneuverability, as well as the proportionality constant ($N'$) for the pursuers. It features proportional navigation implemented in two different ways: zero-effort miss (ZEM) and line-of-sight (LOS).  

Made with Unity. The guidance laws are from Ben Dickinson's ["Guidance Fundamentals Tutorial"](https://www.youtube.com/playlist?list=PLcmbTy9X3gXt02z1wNy4KF5ui0tKxdQm7) series on Youtube and Paul Zarchan's [Tactical and Strategic Missile Guidance (6th ed)](https://arc.aiaa.org/doi/10.2514/4.868948).   

In the video examples below, the green pursuer always attempts to reach where the target (red) currently is, it is essentially a control experiment. 

--- 
### Zero-Effort Miss
Tries to minimize the miss-distance if trajectories stop changing. The equation for this law is found on Zarchan's pg.33:

$$acceleration = \frac{N \times {ZEM}_{PLOS}}{t _{togo}^2}$$

https://github.com/user-attachments/assets/a72539b2-e9e5-44e1-b9a5-f2589648b9c0

--- 
### Line-of-Sight
A change in the line-of-sight angle to the target should result in a change to the heading angle of the pursuer. This equation for this law is found on Zarchan's pg.14:

$$acceleration = N \times V_c \times \dot\lambda$$

https://github.com/user-attachments/assets/9a7577f2-78fe-4703-9f28-719bf0b521b7

--- 
### Demo with both
https://github.com/user-attachments/assets/ee0c18b3-a27e-4e48-8ac2-ec65c074ef6e

