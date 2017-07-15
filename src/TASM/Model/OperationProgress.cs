// -----------------------------------------------------------------------
// <copyright file="OperationProgress.cs" company="Microsoft">
//      Copyright (c) Microsoft Corporation.  All rights reserved.
// </copyright>
// -----------------------------------------------------------------------

namespace TASM.Model
{
    using System;

    /// <summary>
    /// Class that is used for tracking Operation Progress for purpose of display progress and status on the UI.
    /// </summary>
    public class OperationProgress
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OperationProgress"/> class.
        /// </summary>
        public OperationProgress()
        {
            this.OperationInProgressMsg = string.Empty;
            this.OverallTotalSteps = 0;
            this.OverallCurrentStep = 0;
            this.OperationinProgressTotalSteps = 0;
            this.OperationinProgressCurrentStep = 0;
            this.OperationinProgressSubStepNumber = 0;
            this.IsRunning = false;
        }

        /// <summary>
        /// Gets or sets the Message to be displayed when operation is in progress.
        /// </summary>
        public string OperationInProgressMsg { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the operation is running or not.
        /// </summary>
        private bool IsRunning { get; set; }

        /// <summary>
        /// Gets or sets the total number of steps for measuring overall progress.
        /// </summary>
        private int OverallTotalSteps { get; set; }

        /// <summary>
        /// Gets or sets step number of the step in progress.
        /// </summary>
        private int OverallCurrentStep { get; set; }

        /// <summary>
        /// Gets or sets total number of sub-steps for the operation in progress.
        /// </summary>
        private int OperationinProgressTotalSteps { get; set; }

        /// <summary>
        /// Gets or sets step number of the step in progress.
        /// </summary>
        private int OperationinProgressCurrentStep { get; set; }

        /// <summary>
        /// Gets or sets sub-step number of the step in progress.
        /// </summary>
        private int OperationinProgressSubStepNumber { get; set; }

        /// <summary>
        /// Gets the overall progress percentage 
        /// </summary>
        /// <returns> Returns the overall progress percentage </returns>
        public double GetOverallProgress()
        {
            double overallProg = 0;
            if (!this.IsRunning)
            {
                overallProg = 100;
            }
            else
            {
                try
                {
                    overallProg = (this.OverallCurrentStep - 1) * (100 / (double)this.OverallTotalSteps);
                    if (this.OperationinProgressTotalSteps > 0)
                    {
                        overallProg = overallProg + (this.OperationinProgressCurrentStep * (100 / (double)this.OverallTotalSteps) / this.OperationinProgressTotalSteps);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            
            return overallProg;
        }

        /// <summary>
        /// Gets the current step as a string
        /// </summary>
        /// <returns> Returns the current step as a string</returns>
        public string GetCurrentStep()
        {
            string currentStepStr = string.Empty;
            try
            {
                currentStepStr = UIStatusMessages.Step + this.OverallCurrentStep.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return currentStepStr;
        }

        /// <summary>
        /// Gets the current step including sub step number as a string
        /// </summary>
        /// <returns> Returns the current step including sub step number as a string</returns>
        public string GetCurrentSubStep()
        {
            string currentStepSubStr = string.Empty;
            try
            {
                currentStepSubStr = this.GetCurrentStep() + UIStatusMessages.SubStepDelimiter + this.OperationinProgressSubStepNumber.ToString();
            }
            catch (Exception)
            {
                throw;
            }

            return currentStepSubStr;
        }

        /// <summary>
        /// Moves the progress to next step
        /// </summary>
        /// <param name="opinProgressStrTemplate">The message for next operation in progress</param>
        /// <param name="opinProgressTotalSteps">Total steps in next operation</param>
        public void NextStep(string opinProgressStrTemplate, int opinProgressTotalSteps)
        {
            this.OverallCurrentStep++;
            this.OperationinProgressCurrentStep = 0;
            this.OperationinProgressSubStepNumber = 1;
            this.OperationInProgressMsg = string.Format(opinProgressStrTemplate, this.GetCurrentStep());
            this.OperationinProgressTotalSteps = opinProgressTotalSteps;
        }

        /// <summary>
        /// Initiates the progress tracking
        /// </summary>
        /// <param name="overallTotalSteps">Total steps in overall progress</param>
        public void Start(int overallTotalSteps)
        {
            this.IsRunning = true;
            this.OverallTotalSteps = overallTotalSteps;
            this.OverallCurrentStep = 0;
        }

        /// <summary>
        /// Moves the progress to next step within an operation
        /// </summary>
        public void IncrementOpinProgressStep()
        {
            this.OperationinProgressCurrentStep++;
        }

        /// <summary>
        /// Moves the progress to next sub-step within an operation
        /// </summary>
        public void IncrementSubStepNumber()
        {
            this.OperationinProgressSubStepNumber++;
        }

        /// <summary>
        /// Stops the progress tracking
        /// </summary>
        public void Stop()
        {
            this.OperationInProgressMsg = string.Empty;
            this.IsRunning = false;
        }
    }
}
