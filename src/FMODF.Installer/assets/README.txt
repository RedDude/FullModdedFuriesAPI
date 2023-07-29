     ___           ___           ___           ___        ___
    /  /\         /__/\         /  /\         /  /\      /  /\
   /  /:/_       |  |::\       /  /::\       /  /::\    /  /:/
  /  /:/ /\      |  |:|:\     /  /:/\:\     /  /:/\:\  /  /:/
 /  /:/ /::\   __|__|:|\:\   /  /:/~/::\   /  /:/~/:/ /  /::\ ___
/__/:/ /:/\:\ /__/::::| \:\ /__/:/ /:/\:\ /__/:/ /:/ /__/:/\:\  /\
\  \:\/:/~/:/ \  \:\~~\__\/ \  \:\/:/__\/ \  \:\/:/  \__\/  \:\/:/
 \  \::/ /:/   \  \:\        \  \::/       \  \::/        \__\::/
  \__\/ /:/     \  \:\        \  \:\        \  \:\        /  /:/
    /__/:/       \  \:\        \  \:\        \  \:\      /__/:/
    \__\/         \__\/         \__\/         \__\/      \__\/


FMODF lets you run Full Metal Furies with mods. Don't forget to download mods separately.


Player's guide
--------------------------------
See https://stardewvalleywiki.com/Modding:Player_Guide for help installing FMODF, adding mods, etc.


Manual install
--------------------------------
THIS IS NOT RECOMMENDED FOR MOST PLAYERS. See instructions above instead.
If you really want to install FMODF manually, here's how.

1. Unzip "internal/windows/install.dat" (on Windows) or "internal/unix/install.dat" (on
   Linux/macOS). You can change '.dat' to '.zip', it's just a normal zip file renamed to prevent
   confusion.
2. Copy the files from the folder you just unzipped into your game folder. The
   `FullModdedFuriesAPI.exe` file should be right next to the game's executable.
3.
  - Windows only: if you use Steam, see the install guide above to enable achievements and
    overlay. Otherwise, just runFullModdedFuriesAPI.exe in your game folder to play with mods.

  - Linux/macOS only: rename the "Brawler2D" file (no extension) to "Brawler2D-original", and
    "FullModdedFuriesAPI" (no extension) to "Brawler2D". Now just launch the game as usual to
    play with mods.

When installing on Linux or macOS:
- Make sure Mono is installed (normally the installer checks for you). While it's not required,
  many mods won't work correctly without it. (Specifically, mods which load PNG images may crash or
  freeze the game.)
- To configure the color scheme, edit the `fmodf-internal/config.json` file and see instructions
  there for the 'ColorScheme' setting.
