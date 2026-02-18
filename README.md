# Antique Store Tycoon README.
An isometric pixel art tycoon game that sells ancient artifacts to ghost customers.

References:
- [Documentation](https://docs.google.com/document/d/1t01sbBmrGm1AgO76bAs0oLU08kyMyzSFqB_mYw1m0dE/edit?tab=t.0)
- [Mastersheet](https://docs.google.com/spreadsheets/d/1iFkrWjrHKfmh67OTsAloo9MOUdXhFeanlY9jQFFxcsg/edit?gid=1152636205#gid=1152636205)

## Branching Rules
 **All branches should reference from "main"**

 Create a new branch from main
```git switch -c UI/UX-MainLevel```

 Commit your changes
```git commit -m "feat: added UI"```

 Push to main ***(Ensure your branch has no conflicts or errors)***
```git push origin main UI/UX-MainLevel```

### Project Structure ***(UNITY VERSION MUST BE 6000.3.2f1)***

```GameFolder/
├── Assets/
│   ├── Art/
│   │   ├── Materials/
│   │   ├── Sprites/
│   │   ├── Textures/
│   |   ├── UI/
|   |   └── Visual Effects/
│   ├── Audio/
│   │   ├── SFX/
│   │   └── Music/
|   ├── Scenes/
|   ├── Scripts/
│   │   ├── GameManager/
|   |   ├── Gameplay/
|   |   └── UI
```
