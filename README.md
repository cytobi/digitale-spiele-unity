# digitale-spiele-unity

exercises for digitale spiele lecture

repo set up for unity development using github, git lfs & unity 2021.3.26f1

## additional setup

add the following to your local git config to use unity smart merge:
```
[merge]
    tool = unityyamlmerge
    
[mergetool "unityyamlmerge"]
    trustExitCode = false
    cmd = '<path to UnityYAMLMerge>' merge -p "$BASE" "$REMOTE" "$LOCAL" "$MERGED"
```
where `<path to UnityYAMLMerge>` is the path to your local install of UnityYAMLMerge (shipped with unity editor).  
the path may look something like `C:\Program Files\Unity\Editor\Data\Tools\UnityYAMLMerge.exe`
