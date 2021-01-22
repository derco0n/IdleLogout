# IdleLogout
A Windows-Service which terminates disconnected user sessions periodically

## Problem:
Windows allows you to login multiple user-sessions on a machine. While the last active interactive session stays "connected",
the older/idle sessions become "disconnected" but will keep using CPU and RAM-ressources as the user's applications are still active.
In environments where a lot of different users frequently use the same machine this can cause issues.

Sometimes you might need to terminate disconnected user sessions only keeping the session of the last active user alive.
Microsoft didn't offer a group policy for this szenario and i don't know about any registry setting that could be applied to achieve this.

## Solution:
This windows-service will periodally check for "disconnected" interactive sessions and closes them afterwards - which effectively frees RAM and CPU ressources.
It will leave the last active (not "disconnected") session alive.
