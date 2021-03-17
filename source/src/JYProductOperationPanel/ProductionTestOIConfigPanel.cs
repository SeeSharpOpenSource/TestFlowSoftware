using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Testflow.Data.Sequence;
using Testflow.Runtime.OperationPanel;

namespace JYProductOperationPanel
{
    public class ProductionTestOIConfigPanel : IOIConfigPanel
    {
        private ProductionTestOIConfigForm _configForm;
        private string[] _availableDirs;

        public ProductionTestOIConfigPanel()
        {
            
        }

        public void ShowOiConfigPanel(ISequenceFlowContainer sequenceData, params object[] extraParams)
        {
            ISequenceGroup sequenceGroup = sequenceData as ISequenceGroup;
            string[] variableNames = sequenceGroup.Variables.Select(item => item.Name).ToArray();
            string assemblyPath = string.Empty;
            string panelClassName = string.Empty;
            string messageVariableName = string.Empty;

            string[] availableDirs = extraParams[0] as string[];
            string[] paramElements = null;
            string parameter = sequenceGroup.Info.OperationPanelInfo?.Parameters ?? string.Empty;
            this._availableDirs = availableDirs;
            if (!string.IsNullOrWhiteSpace(parameter) && (paramElements = parameter.Split('$')).Length == 3)
            {
                assemblyPath = Utility.TryGetAbsolutePath(paramElements[0], this._availableDirs);
                if (!string.IsNullOrWhiteSpace(assemblyPath))
                {
                    panelClassName = paramElements[1];
                }
                messageVariableName = paramElements[2];
            }

            this._configForm = new ProductionTestOIConfigForm(assemblyPath, panelClassName, messageVariableName, variableNames);
            this._configForm.ShowDialog();
        }

        public string GetParameter(out bool isConfirmed)
        {
            isConfirmed = this._configForm.IsConfirmed;
            if (!this._configForm.IsConfirmed)
            {
                return string.Empty;
            }
            string assemblyPath, panelClassName, messageVariableName;
            this._configForm.GetParameter(out assemblyPath, out panelClassName, out messageVariableName);
            if (string.IsNullOrWhiteSpace(assemblyPath)) return string.Empty;
            assemblyPath = Utility.GetRelativePath(assemblyPath, this._availableDirs);
            return $"{assemblyPath}${panelClassName}${messageVariableName}";
        }

    }
}