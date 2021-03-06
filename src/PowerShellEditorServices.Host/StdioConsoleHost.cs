﻿//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.PowerShell.EditorServices.Protocol.MessageProtocol;
using Microsoft.PowerShell.EditorServices.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices.Host
{
    public class StdioConsoleHost : IConsoleHost
    {
        #region Private Fields

        private MessageWriter messageWriter;
        private int currentReplEventSequence = 0;
        private TaskCompletionSource<int> currentPromptChoiceTask;

        #endregion

        #region Constructors

        public StdioConsoleHost(MessageWriter messageWriter)
        {
            Validate.IsNotNull("messageWriter", messageWriter);

            this.messageWriter = messageWriter;
        }

        #endregion

        #region IConsoleHost Implementation

        Task<int> IConsoleHost.PromptForChoice(
            string caption, 
            string message, 
            IEnumerable<ChoiceDetails> choices,
            int defaultChoice)
        {
            // NOTE: This code is held temporarily until a new model is
            // found for dealing with interactive prompts.

            //// Create and store a TaskCompletionSource that will be
            //// used to send the user's response back to the caller
            //this.currentPromptChoiceTask = new TaskCompletionSource<int>();
            //this.currentReplEventSequence++;

            //this.messageWriter.WriteMessage(
            //    new ReplPromptChoiceEvent
            //    {
            //        Body = new ReplPromptChoiceEventBody
            //        {
            //            Seq = this.currentReplEventSequence,
            //            Caption = caption,
            //            Message = message,
            //            DefaultChoice = defaultChoice,
            //            Choices = 
            //                choices
            //                    .Select(ReplPromptChoiceDetails.FromChoiceDescription)
            //                    .ToArray()
            //        }
            //    });

            //return this.currentPromptChoiceTask.Task;

            throw new NotImplementedException("This method is currently being refactored and is not available.");
        }

        void IConsoleHost.PromptForChoiceResult(
            int promptId,
            int choiceResult)
        {
            // TODO: Validate that prompt ID exists
            Validate.IsNotNull("currentPromptChoiceTask", this.currentPromptChoiceTask);

            this.currentPromptChoiceTask.SetResult(choiceResult);
            this.currentPromptChoiceTask = null;
        }

        void IConsoleHost.UpdateProgress(
            long sourceId, 
            ProgressDetails progressDetails)
        {
            // TODO: Implement message for this
        }

        void IConsoleHost.ExitSession(int exitCode)
        {
            // TODO: Implement message for this
        }

        public void WriteOutput(
            string outputString, 
            bool includeNewLine = true, 
            OutputType outputType = OutputType.Normal, 
            ConsoleColor foregroundColor = ConsoleColor.White, 
            ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            // This is taken care of elsewhere now.  This interface will
            // be refactored out in the near future.
        }

        #endregion
    }
}
