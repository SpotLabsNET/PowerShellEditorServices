﻿//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
//

using Microsoft.Windows.PowerShell.ScriptAnalyzer;
using System;
using System.Linq;
using System.Management.Automation.Runspaces;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.PowerShell.EditorServices
{
    /// <summary>
    /// Provides a high-level service for performing semantic analysis
    /// of PowerShell scripts.
    /// </summary>
    public class AnalysisService : IDisposable
    {
        #region Private Fields

        private Runspace analysisRunspace;
        private ScriptAnalyzer scriptAnalyzer;

        #endregion

        #region Constructors

        /// <summary>
        /// Creates an instance of the AnalysisService class.
        /// </summary>
        public AnalysisService()
        {
            // TODO: Share runspace with PowerShellContext?  Probably should always
            //       run analysis in a local session.
            this.analysisRunspace = RunspaceFactory.CreateRunspace(InitialSessionState.CreateDefault2());
            this.analysisRunspace.ApartmentState = ApartmentState.STA;
            this.analysisRunspace.ThreadOptions = PSThreadOptions.ReuseThread;
            this.analysisRunspace.Open();

            this.scriptAnalyzer = new ScriptAnalyzer();
            this.scriptAnalyzer.Initialize(
                this.analysisRunspace,
                new AnalysisOutputWriter(),
                null,
                null,
                new string[] { "DscTestsPresent", "DscExamplesPresent" });
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Performs semantic analysis on the given ScriptFile and returns
        /// an array of ScriptFileMarkers.
        /// </summary>
        /// <param name="file">The ScriptFile which will be analyzed for semantic markers.</param>
        /// <returns>An array of ScriptFileMarkers containing semantic analysis results.</returns>
        public ScriptFileMarker[] GetSemanticMarkers(ScriptFile file)
        {
            if (file.IsAnalysisEnabled)
            {
                // TODO: This is a temporary fix until we can change how
                // ScriptAnalyzer invokes their async tasks.
                Task<ScriptFileMarker[]> analysisTask =
                    Task.Factory.StartNew<ScriptFileMarker[]>(
                        () =>
                        {
                            return 
                                this.scriptAnalyzer
                                    .AnalyzeSyntaxTree(
                                        file.ScriptAst,
                                        file.ScriptTokens,
                                        file.FilePath)
                                    .Select(ScriptFileMarker.FromDiagnosticRecord)
                                    .ToArray();
                        },
                        CancellationToken.None,
                        TaskCreationOptions.None,
                        TaskScheduler.Default);

                return analysisTask.Result;
            }
            else
            {
                // Return an empty marker list
                return new ScriptFileMarker[0];
            }
        }

        /// <summary>
        /// Disposes the runspace being used by the analysis service.
        /// </summary>
        public void Dispose()
        {
            if (this.analysisRunspace != null)
            {
                this.analysisRunspace.Close();
                this.analysisRunspace.Dispose();
                this.analysisRunspace = null;
            }
        }

        #endregion
    }
}
