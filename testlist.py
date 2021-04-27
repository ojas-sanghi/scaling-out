def main():
    directory = "C:\\Users\\Sajo\\Desktop\\saj\\GODOT\\scaling-out"
    allfiles = []
    files = list_files(directory, "tscn")
    allfiles.extend(files)
    print(allfiles)

    for af in allfiles:
        print(f"{af} | -")

from os import walk
 
def list_files(directory, extension):
    fies = []
    for (dirpath, dirnames, filenames) in walk(directory):
        for f in filenames:
            dirpath: str = dirpath
            path = dirpath[32:]
            if f.endswith("." + extension):
                fies.append(path + "\\" + f)
    return fies

main()