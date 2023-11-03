﻿using Nuke.Common;
using Nuke.Common.Execution;

[DisableDefaultOutput<Terminal>(
    DefaultOutput.Logo
    //DefaultOutput.Timestamps,
    //DefaultOutput.TargetHeader,
    //DefaultOutput.ErrorsAndWarnings,
    //DefaultOutput.TargetOutcome,
    //DefaultOutput.BuildOutcome
    )]
public partial class Build
{
}

public class DisableDefaultOutputAttribute<T> : DisableDefaultOutputAttribute
    where T : Host
{
    public DisableDefaultOutputAttribute(params DefaultOutput[] disabledOutputs)
        : base(disabledOutputs)
    {
    }

    public override bool IsApplicable(INukeBuild build) => build.Host is T;
}

