//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol;

namespace Microsoft.PowerShell.EditorServices.Protocol.DebugAdapter
{
    public class StepOutRequest
    {
        public static readonly
            RequestType<object, object, object> Type =
            RequestType<object, object, object>.Create("stepOut");
    }
}
