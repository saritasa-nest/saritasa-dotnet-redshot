﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace RedShot.Recording.Helpers
{
    public enum FFmpegX264Preset
    {
        [Description("Ultra fast")]
        ultrafast,
        [Description("Super fast")]
        superfast,
        [Description("Very fast")]
        veryfast,
        [Description("Faster")]
        faster,
        [Description("Fast")]
        fast,
        [Description("Medium")]
        medium,
        [Description("Slow")]
        slow,
        [Description("Slower")]
        slower,
        [Description("Very slow")]
        veryslow
    }
}
