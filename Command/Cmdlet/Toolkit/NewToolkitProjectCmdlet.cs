﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Threading.Tasks;

namespace AstralKeks.Workbench.Command
{
    [Cmdlet(VerbsCommon.New, Noun.WBToolkitProject)]
    public class NewToolkitProjectCmdlet : WorkbenchDynamicCmdlet
    {
    }
}
