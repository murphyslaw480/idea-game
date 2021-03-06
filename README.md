This is the README in the Git repository for the currently unnamed IDEA
game!

## Plan for the game

The theme is "unicorns and leprechauns in space".

### Gameplay synopsis

Kill the dangerous unicorns and leprechauns, who are trying to murder
you, whilst avoiding being sucked into the nearby black hole.

## How to use Git and Github

### Getting your own copy of the repository

1.  Make a Github account and install [Github for Windows][2] (or just
    Git on Mac/Linux).

2.  Visit the [main idea-game repository][3] on github.com and click the
    **Fork** button. Now you have a copy of the repository under your
    Github account.

3.  Now you need to **clone** your forked repository to your computer. You
    can do this through Github for Windows, or you can open the Git
    shell, `cd` to a suitable location, and do

        $ git clone <repository>

    where `<repository>` is something like
    `git@github.com:<username>/<reponame>` (this is shown on the Github
    page for your repository). This will create a new directory with the
    same name as the repository.

4.  Add the `upstream` remote. I think you have to use the Git shell to
    do this.

        $ git remote add upstream git://github.com/kalgynirae/idea-game.git

### Git basics

Your best friend:

    $ git status

#### Staging and committing

After you've made changes to files in the repository, you have to
*stage* and then *commit* the changes.

**Staging** lets you specify exactly which changes you want to commit.
For example, if you change File A and File B, but the changes are
unrelated and would make more sense in separate commits, you can stage
File A, commit, and then stage File B and commit again. Usually you'll
stage entire files, but it's also possible to stage only parts of files.

In the Git shell, you can stage files like this:

    $ git add <files>

To see what files have been changed and what files are currently staged,
use:

    $ git status

To see what has changed in a particular file:

    $ git diff <file>

**Committing** is the process of packaging up the changes you've made
with a message that explains them.

Commiting is done in the Git shell like this:

    $ git commit

This will open a text editor so that you can enter your commit message.
Once you save the file and quit the editor, Git will record the commit.
If you want to cancel the commit, you can exit the editor without saving
the file.

Git commit messages consist of a required short title
(written in the imperative voice, max 55 characters) and an optional
longer description.

Commits in Git are identified by a hexidecimal hash. The hashes are
quite long, but in most cases you can use just the first 6 or 7 characters of
the hash.

To see the log of previous commits and their hashes:

    $ git log
    $ git log --oneline

#### Branches

The default branch in a Git repository is called `master`.

To list the branches in your local repository or to see which branch you
currently have checked out, you can do:

    $ git branch

If you also want to see local copies of remote branches:

    $ git branch -a

To switch to a branch:

    $ git checkout <branch name>

To make a new branch:

    $ git checkout -b <branch name>

#### Remotes and pushing, fetching, and merging

A **remote** in Git is simply a named online repository. When you clone
a repository (like you did to get a local copy of your repo on
github.com), Git automatically sets up a remote called `origin`. In the
case where you are working on a project that has a main repository, you
typically add that main repo as a remote called `upstream`.

---

To send commits from your local repository to an online repository, you
**push** to the remote for the online repository.

    $ git push <remote> <optional branch name>

Typically, `<remote>` will be `origin` and the branch name will be
omitted, although if you want to push a newly-created branch then you do
have to specify the branch name.

---

**Fetching** is the process of downloading changes from a remote repo to
your computer. Your local repository keeps copies of remote
branches, but these don't automatically stay updated. To update them,
you fetch from the corresponding remote repo.

    $ git fetch <remote>

---

To incorporate the changes from Branch A into Branch B, you
**merge** Branch A into Branch B.

    $ git checkout <branch b>
    $ git merge <branch a>

There are other ways to incorporate changes from one branch into
another, but merging is the simplest and most common.

### Using Git to work on the game

In general, you'll want to develop individual features on separate
branches. In practice, it's not a big deal what branch you work on,
because it's pretty easy to move commits between branches later.
However, if you know what you're planning to do, you should start by
creating a new branch.

As you work, you should periodically stage and commit your changes. You
don't need to worry too much about making your commits consistent or
about making too many or too few commits, because commits can be
combined or re-worked later if needed.

[1]: https://github.com/
[2]: http://windows.github.com/
[3]: https://github.com/kalgynirae/idea-game
