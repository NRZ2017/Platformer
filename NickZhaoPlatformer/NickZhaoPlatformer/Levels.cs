using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NickZhaoPlatformer
{
    static class Levels 
    {
        static public void Level1(List<Platform> platform, List<Spikes> spikes, List<SolidPlatform> solidPlatforms, Texture2D platformImage, Texture2D spikesImage, Texture2D Background)
        {
            platform.Add(new Platform(new Vector2(276, 912), platformImage, Color.White, 1.5f, true, true));
            platform.Add(new Platform(new Vector2(626, 697), platformImage, Color.White, 1.5f, true, true));
            platform.Add(new Platform(new Vector2(1013, 520), platformImage, Color.White, 1.5f, true, false));
            platform.Add(new Platform(new Vector2(1391, 280), platformImage, Color.White, 1.5f, true, true));
            platform.Add(new Platform(new Vector2(1728, 50), platformImage, Color.White, 1.5f, false, true));
            solidPlatforms.Add(new SolidPlatform(new Vector2(500, 500), platformImage, Color.White, 1.5f, false));
            spikes.Add(new Spikes(new Vector2(562, 620), spikesImage, Color.White, 0.5f, true, true));
            spikes.Add(new Spikes(new Vector2(1326, 205), spikesImage, Color.White, 0.5f, false, true));
            spikes.Add(new Spikes(new Vector2(1391, 205), spikesImage, Color.White, 0.5f, false, true));
            spikes.Add(new Spikes(new Vector2(1456, 205), spikesImage, Color.White, 0.5f, false, true));
        }
        

        
    }
}
