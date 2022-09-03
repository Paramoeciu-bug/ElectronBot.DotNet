﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronBot.BraincasePreview.Picker
{
    public interface IObjectPicker<T>
    {
        event EventHandler<ObjectPickedEventArgs<T>> ObjectPicked;
        event EventHandler Canceled;
    }
}
