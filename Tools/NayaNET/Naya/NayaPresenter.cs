//-----------------------------------------------------------------------
// <copyright file="NayaPresenter.cs" company="none">
// Copyright (c) kenny-nelson All Rights Reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace Naya
{    
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Dodai;
    using Dodai.Bridges;
    using Dodai.Logs;
    using Dodai.Menus;
    using Dodai.Operations;
    using Dodai.Panels;
    using Dodai.Plugins;
    using Dodai.Repositories;
    using Dodai.Services;

    internal sealed class NayaPresenter : GlobalPresenter
    {
        internal void AddPanelPresenter(IPanelPresenter panelPresenter)
        {
            this.AddPresenter(GlobalPresenter.PanelPresenterKey, panelPresenter);
        }

        internal void AddPluginPresenter(IPluginPresenter pluginPresenter)
        {
            this.AddPresenter(GlobalPresenter.PluginPresenterKey, pluginPresenter);
        }

        internal void AddLogPresenter(ILogPresenter logPresenter)
        {
            this.AddPresenter(GlobalPresenter.LogPresenterKey, logPresenter);
        }

        internal void AddOperationPresenter(IOperationPresenter operationPresenter)
        {
            this.AddPresenter(GlobalPresenter.OperationPresenterKey, operationPresenter);
        }

        internal void AddMenuPresenter(IMenuPresenter menuPresenter)
        {
            this.AddPresenter(GlobalPresenter.MenuPresenterKey, menuPresenter);
        }

        internal void AddRepositoryPresenter(IRepositoryPresenter repositoryPresenter)
        {
            this.AddPresenter(GlobalPresenter.RepositoryPresenterKey, repositoryPresenter);
        }

        internal void AddScriptPresenter(IScriptPresenter scriptPresenter)
        {
            this.AddPresenter(GlobalPresenter.ScriptPresenterKey, scriptPresenter);
        }

        internal void AddPresenter(string presenterKey, IPresenter presenter)
        {
            this.AddPresenterInternal(presenterKey, presenter);
        }
    }
}
