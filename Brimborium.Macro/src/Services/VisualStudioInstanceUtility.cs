﻿using Microsoft.Build.Locator;

using System.Runtime.CompilerServices;

namespace Brimborium.Macro;

public static class VisualStudioInstanceUtility {
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static VisualStudioInstance? RegisterInstance() {
        var instance = GetVisualStudioInstance();
        if (instance is not null) {
            MSBuildLocator.RegisterInstance(instance);
        }
        return instance;
    }

    public static VisualStudioInstance? GetVisualStudioInstance() {
        var visualStudioInstances = MSBuildLocator.QueryVisualStudioInstances().ToArray();
        if (visualStudioInstances.Length == 0) {
            return null;
        }
        if (visualStudioInstances.Length == 1) {
            return visualStudioInstances[0];
        }
        return SelectHighestVisualStudioInstance(visualStudioInstances);
    }

    public static VisualStudioInstance SelectHighestVisualStudioInstance(
        VisualStudioInstance[] visualStudioInstances
        ) {
        var result = visualStudioInstances[0];
        for (int index = 1; index < visualStudioInstances.Length; index++) {
            var item = visualStudioInstances[index];
            if (result.Version < item.Version) {
                result = item;
            }
        }
        return result;
    }
}
