# Leap Tetris

## Inspiration

Remember those Japanese game shows with human Tetris? Giant, moving walls would move towards contestants, and they would have to contort their bodies to fit through the holes in the wall or get knocked into the water! You can check out the format here (Click the photo to get taken to a YouTube video!): 

[![IMAGE ALT TEXT HERE](https://img.youtube.com/vi/6ioiMXKpHxI/0.jpg)](https://www.youtube.com/watch?v=6ioiMXKpHxI)

We've been wanting to break into the MLH hardware lab to get our hands on a Leap Motion device for ages, and decided to make a virtual human Tetris for your hands! 

## What it does

Player uses our _groundbreaking innovation_ (Leap Motion device duct taped to the RBC provided sleep-masks) to attach the leap motion on their forehead. The player must move their hand to match the shape of the wall moving towards them in our unity game. The more your hand overlaps with the wall, the more points you lose. The player with the lowest score wins! 

**Hootsuite Social Game Version:**

Player one wears the Leap Motion, and also covers their eyes. Player two must tell them how to move their hand to get through the wall. 

## How we built it

The game was built using Unity/C#. The shapes of the wall were created using Blender. We needed to interact with the Leap Motion API to calculate the users score. This was done by calculating the position of all of the bones in the visible hands, and then calculating the amount of overlap with the wall shapes. So a player that only has one finger hit the wall will get a lower penalty than a player that has their entire hand overlap with the wall.

## Challenges we ran into

One of the hardest parts of this hackathon was figuring out how to calculate the score for the user. We had to work together as a team to conceptually figure out how we could calculate the overlap of the hand with the wall object.

Leap motion exposes an engine to support contacts/collisions between the virtual hand objects and other physics objects within Unity, but we had the additional challenge of computing "severity" of overlap. A player with 50% of their hand overlapping the incoming Tetris shape should receive a worse score than one who just has an overlapping pinky. We ended up calculating this severity based on "number of contacting digits", as the Leap API exposes the collison state of each finger/palm segment on the hand rig. In order to measure overlap continuously, we didn't use the visual object, but rather a virtual collision object that extends all the way to the user's hands at all time. We switch this collision objects as new Tetris pieces come into focus.

Another challenge we had came from more of a "game design" perspective, where we had to carefully tune the scale and speed of the Tetris pieces, as well as the positioning and field of view of the game camera to give the perception of depth to the oncoming pieces and offer a comfortable level of challenge. 

## Accomplishments that we're proud of

We are really happy with how this app turned out! None of us have any prior experience with the Leap Motion, and 2/3 of us have never done any game development or have any experience with Unity. Getting the scoring working was a close call, so we are glad we managed to finish that in time!

## What we learned

We learned lots about game development, and the possibilities and limitations of working with the Leap Motion device.

## What's next for Leap Tetris  

We'd be excited to try to make a version of the Fruit Ninja game using the Leap Motion! 
