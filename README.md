# gedfmt

The gedfmt project supports text formatting related to genealogy 'gedcom' files.

The initial drive for the project was to get more reasonably formatted output from
WikiTree.com's gedcom import tool, to reduce manual toil when adding many profiles.

## Directories

### windows_desktop

Otherwise known as "WikiTreeTextFormatter".

This is a Windows desktop application created in 2017 by [David Loring](https://www.wikitree.com/wiki/Meredith-1182). \
It is documented at https://www.wikitree.com/wiki/Space:WikiTree_Text_Formatter. [archive_link](https://web.archive.org/web/20210922065456/https://www.wikitree.com/wiki/Space:WikiTree_Text_Formatter) \
It is part of this repository because other work is based closely on it, and it was not previously on
github. David has given permission to have it here.

### windows_gedfmt

An executable file with support for formatting the content of WikiTree gedcom-generated profiles.
It is a hacked together version of `WikiTreeTextFormatter`, but with the GUI removed.

Sample usage:

```shell
$ gedfmt.exe path/to/file.txt
```

It will generate a file named `path/to/file_fmt.txt` (note the _fmt before the extension).

### server

Server is a Golang executable that exposes an HTTP interface for formatting content.
It also serves a very minimal webpage.
