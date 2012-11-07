## Game description

### Backstory

You are a lone space traveler whose ship has been impaired. Unfortunately, the
parts you need to repair your ship have fallen into the black hole, and you
have no way to get them out.

### Gameplay synopsis

Kill dangerous enemies that are trying to kill you, or trick them into being
sucked into the black hole. If enough things fall into the black hole, it will
violently eject its contents, giving you a chance to recover the ship parts
you need.

### Player mechanics

*   WASD for movement
*   Mouse for aiming
*   Grappling hook (this is extra fun with space physics!)
*   Multi-dimensional magnetic boots that anchor you in place

### Level mechanics

#### Space levels

*   Black hole somewhere on the border that sucks things toward it
*   Enemies spawning around the border of the screen. See types of enemies
    below.

#### Non-space levels ??

### Specific enemies

#### Goblin (in space suits?)

The typical, basic grunt unit.

#### Leprechaun

Ranged unit that attacks by throwing coins at you.

#### Unicorn

Relatively neutral unit that appears on a side of the screen, prepares, and
then charges across the screen, damaging anything in his path and leaving
behind a temporary rainbow bridge that can be utilized to quickly travel
across the screen. It may or may not be possible to latch onto one with your
grappling hook, and even if it is, the effects may be deadly...

#### Ogers

Heavy unit that has a slight gravitational pull and possibly also a large
attack that can dangerously launch other enemies at you.

## How to work on code for the game

Assuming you have already forked and cloned the repository and set up
the upstream remote (if not, read the sections below):

1.  Download any changes from upstream.

        $ git fetch upstream

2.  Start a new branch based on upstream/master.

        $ git checkout upstream/master
        $ git checkout -b newbranchname

3.  Start coding! Commit often!

4.  Push your commits to your Github repository so others can look at them.

### Useful Git commands

    # BEST COMMAND EVER
    $ git status

    # Stage changed files
    $ git add <file>

    # Commit staged changes
    $ git commit -m "<commit message>"

    # List all branches on your computer
    $ git branch -a

    # Check out a branch
    $ git checkout

    # Make a new branch based on the currently-checked-out branch
    $ git checkout -b <new branch name>

    # Display the commit log
    $ git log

    # Add a named remote repository
    $ git remote add <remote name> <remote url>

    # Download updates from a remote repository
    $ git fetch <remote name>

    # Rebase your current branch on top of another branch
    # This is most likely what you should do if you have made some commits
    # but there are new commits in the master repository that you want to
    # include.
    $ git rebase <branch name>
