using System.Collections;
using System.Collections.Generic;
using System.Xml;

namespace System.Web.Core
{
    public class Command
    {
        #region Private Members

        private string commandIdent;
        private string commandName;
        private List<CommandParameter> leafParameters = new List<CommandParameter>();
        private List<CommandParameter> parameters = new List<CommandParameter>();
        private List<CommandParameter> inputParameters = new List<CommandParameter>();
        private List<CommandParameter> outputParameters = new List<CommandParameter>();
        private List<ReturnEntity> returnEntities = new List<ReturnEntity>();

        #endregion

        #region Constructors

        public Command()
        {
        }
        public Command(XmlNode commandNode)
        {
            commandIdent = commandNode.Attributes["commandId"].Value;
            commandName = commandNode.Attributes["commandName"].Value;
            foreach (XmlNode parameterNode in commandNode.SelectSingleNode("parameters").ChildNodes)
            {
                ProcessParameterNode(parameterNode, null, Parameters);
            }

            XmlNode returnEntityCollectionNode = commandNode.SelectSingleNode("returnEntityCollection");
            if (returnEntityCollectionNode != null)
            {
                foreach (XmlNode returnEntityNode in returnEntityCollectionNode.ChildNodes)
                {
                    ReturnEntities.Add(CreateReturnEntity(returnEntityNode));
                }
            }
        }

        #endregion

        #region Public Properties

        public string CommandIdent
        {
            get
            {
                return commandIdent;
            }
        }
        public string CommandName
        {
            get
            {
                return commandName;
            }
        }
        public List<CommandParameter> LeafParameters
        {
            get
            {
                return leafParameters;
            }
        }
        public List<CommandParameter> Parameters
        {
            get
            {
                return parameters;
            }
        }
        public List<CommandParameter> InputParameters
        {
            get
            {
                return inputParameters;
            }
        }
        public List<CommandParameter> OutputParameters
        {
            get
            {
                return outputParameters;
            }
        }
        public List<ReturnEntity> ReturnEntities
        {
            get
            {
                return returnEntities;
            }
        }

        #endregion

        #region Public Methods

        public CommandParameter GetParameter(string propertyPath)
        {
            CommandParameter returnParameter = null;
            foreach (CommandParameter parameter in Parameters)
            {
                returnParameter = GetParameter(parameter, propertyPath);
                if (returnParameter != null)
                {
                    break;
                }
            }
            return returnParameter;
        }

        #endregion

        #region Private Methods

        private void ProcessParameterNode(XmlNode currentXmlNode, CommandParameter parent, List<CommandParameter> parameters)
        {
            CommandParameter currentCommandParameter = new CommandParameter();

            currentCommandParameter.PropertyName = currentXmlNode.Attributes["propertyName"].Value;
            if (parent == null)
            {
                currentCommandParameter.Parent = null;
                currentCommandParameter.PropertyPath = currentCommandParameter.PropertyName;
            }
            else
            {
                currentCommandParameter.Parent = parent;
                currentCommandParameter.PropertyPath = parent.PropertyPath + "." + currentCommandParameter.PropertyName;
            }

            if (currentXmlNode.ChildNodes.Count > 0)
            {
                parameters.Add(currentCommandParameter);
                foreach (XmlNode childNode in currentXmlNode.ChildNodes)
                {
                    ProcessParameterNode(childNode, currentCommandParameter, currentCommandParameter.Childs);
                }
            }
            else
            {
                currentCommandParameter.ParameterName = currentXmlNode.Attributes["parameterName"].Value;
                currentCommandParameter.DbTypeHint = currentXmlNode.Attributes["dbTypeHint"].Value;
                currentCommandParameter.ParamDirection = currentXmlNode.Attributes["paramDirection"].Value;
                parameters.Add(currentCommandParameter);
                leafParameters.Add(currentCommandParameter);
                if (currentCommandParameter.ParamDirection == "Input")
                {
                    inputParameters.Add(currentCommandParameter);
                }
                else if (currentCommandParameter.ParamDirection == "Output")
                {
                    CommandParameter parentParameter = currentCommandParameter;
                    while (parentParameter.Parent != null)
                    {
                        parentParameter = parentParameter.Parent;
                    }
                    outputParameters.Add(parentParameter);
                }
            }
        }
        private CommandParameter GetParameter(CommandParameter parameter, string propertyPath)
        {
            if (parameter.PropertyPath == propertyPath)
            {
                return parameter;
            }

            CommandParameter returnParameter = null;
            foreach (CommandParameter child in parameter.Childs)
            {
                returnParameter = GetParameter(child, propertyPath);
                if (returnParameter != null)
                {
                    return returnParameter;
                }
            }
            return null;
        }
        private ReturnEntity CreateReturnEntity(XmlNode rootReturnEntityNode)
        {
            ReturnEntity rootReturnEntity = new ReturnEntity(rootReturnEntityNode);
            foreach (XmlNode childNode in rootReturnEntityNode.ChildNodes)
            {
                if (childNode.Name == "returnEntity")
                {
                    ProcessReturnEntity(rootReturnEntity, childNode);
                }
            }
            return rootReturnEntity;
        }
        private void ProcessReturnEntity(ReturnEntity parentReturnEntity, XmlNode currentReturnEntityNode)
        {
            ReturnEntity currentReturnEntity = new ReturnEntity(currentReturnEntityNode);
            parentReturnEntity.ChildReturnEntityList.Add(currentReturnEntity);

            foreach (XmlNode childNode in currentReturnEntityNode.ChildNodes)
            {
                if (childNode.Name == "returnEntity")
                {
                    ProcessReturnEntity(currentReturnEntity, childNode);
                }
            }
        }

        #endregion
    }
}
