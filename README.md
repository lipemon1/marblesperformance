# Let's Collect Marbles

My goal here was to improve the project performance so we could inset more marbles, actually, go from 500 to 20,000 at the same time.

# First steps inside the project

When opening the project, the first thing was to try to understand what was going on. After reading what was happening, I separated some behavior that I needed to preserve:

1. Keep the desired number of marbles on screen and ready to be collected.
2. Every actor should be looking for a new marble as soon as they finish collecting theirs.
3. An actor's next target should be the sticky ball closest to him.

## Pooling Marbles because I know, not because they need to

The first thing I did was do what was already clear to me, create a pool of marbles to prevent new balls from being created at runtime.
Was that good? Was. Was it what would help me the most in the project? Not by far. I should have looked at the profile first to find the real bottleneck of increasing the number of marbles but I wanted to start somewhere where I was 100% sure what I was changing.

## Facilitating test changes with Scriptable

So first I created this pool and to facilitate testing, I created a ScriptableObject responsible for configuring the scene.

With this running, I could already have X marbles without needing to instantiate new ones all the time. But, for safety, the pool created increases if by any chance the larger amount of marbles required is larger than the pool has available at that moment.

## Now I'm ready for the problem

Returning to the problem, increasing the number of marbles, and "maintaining" the project's performance. First of all, I went to analyze what was really heavy, and right away I hit the ActorBehavior Update.

To my surprise, UpdateIdle was to blame for more than half of the frame processing. Interesting for a method called Idle. I decided there that I should better understand the method flow inside this little guy. As it was a very small method, I realized right away that my biggest villain would be "find the next marble for every actor".

At first, he searches among all the existing marbles, the closest one, sorts the first 10 by Id, tries to get the first one, and if he does he sets the result of that as his target. Then, if it's a "valid" target, the actor then starts chasing her, changing her status to Hunting. It's a great starting output, very straightforward, easy to read but not very scalable. Well, at least now I had a real villain to face and could start a plan.

## Choosing a Marble to target, that's our villain

Going back to the behaviors above that I had noted and want to keep, I mainly focused on number 3. Find the marble closest to me. With that in mind, I started thinking of ways to get the marbles closest to me without having to search through all the marbles that existed, since most of them would be far away.

That's why I arrived at a structure where each Actor would know about the marbles that are around them and only around them, and when they had to choose a new target ball, each actor instead of looking through all the marbles would look only in those balls around you.

I did this by adding a trigger to each actor who "collects" marbles, meaning they keep a list of every marble that is around them.

## Now that we know our surround, what now?
Great, with that I started a way to increase the amount of processing available, but I still wanted to have more modifications. From there I changed the state of Idle so that he wouldn't look for a new target if he already had one. With that, the structure started to get a little better.

After that, I created an event inside the marbles to know when they were collected. This way, every actor would know the right moment to "lose" (turn null) the current target so that in the next update he would be inside the Idle state and would look for a new target.

With that working things started to get better but even so, the number of searches for new targets were high, using the profile and some debugs I came to the conclusion that this happened mainly with actors that were very close to choosing the same target and when an actor collects the marble that makes the two actors have to look for a new target.

## Let's only target marbles that aren't from others
To prevent this from happening I added a check to the search method where it could only target marbles that weren't targeted by anyone. In addition, I removed some other checks that were no longer needed.

Now yes, I had a structure that was looking for marbles in a more efficient way for a 1000 and 20000 marbles scenario. But, there were still some things that bothered me, two of them mainly, related to the amount of time to process physics and to render.

## Less physic, more fun
To reduce the amount of physics processing I isolated the marbles and actors in different layers and set the physics layers so that the actors could no longer collide, besides, actors that already have a target don't need to keep collecting the possible balls for this at the beginning of each state the object responsible for detecting the marbles around the actor is activated and deactivated. So I managed to decrease the physics processing time a little.

## Rendering, I have failed you
Now my fight was with rendering, and to say it right away, I still haven't won it. I tried to change the shader of the marbles material but there was a minimal difference.

# Final notes

## Next steps

In addition to these changes, I also consider the options below, but I didn't have time to compare and test within the project:
- Instead of using TriggerEnter/Exit to find the marbles around an actor, make an overlapsphere
- Create an overall new target request queue and distribute it across frames to avoid too many actors requesting a new target at the same time.
- Calculate the distance of the marbles using ECS ​​and provide the closest 5 for each actor

## General observations
I really enjoyed doing this exercise, everything that has been done so far was done in 3 days and with a total of 18 hours or so, I don't know if I will continue this test but I would like to see how much I could modify the initial structure in search of more performance.

To better structure the code I made the Scriptable data to be injected by interface so as not to create some kind of external reference.