# Sound-Simulator 2021version
![image](https://user-images.githubusercontent.com/52338219/136432621-136276a0-7e48-46b4-bfe0-ac5393ec2553.png)  
This Branch contains RainHit System and A* PathFinding.  
RainHit is turned off by default. You need to press "R" in game to turn it on.  
Press F1 to open all the doors.  

# Short Explain
PathFinding is typical A* and make it into 3D. A virtual position is caculated by the path. Occlusion is done by raycasting while Obstruction is calculated via the path length.(partly inspired from OverWatch)  
RainHit is inspired from Division2. I just love their GDC showcase so i try to rebuild it from scratch <3. It detected the surface via raycast and group them. After that it decide to add, move or delete.

# Debug details
Crucial scripts are in Assets/2021SpatialAudio  
Only one object is using PathFinding which is TV1.  
