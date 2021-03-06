﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using PSRule.Pipeline;
using PSRule.Resources;
using System.Management.Automation;

namespace PSRule.Commands
{
    /// <summary>
    /// The Recommend keyword.
    /// </summary>
    [Cmdlet(VerbsCommunications.Write, RuleLanguageNouns.Recommendation)]
    internal sealed class WriteRecommendationCommand : RuleKeyword
    {
        [Parameter(Mandatory = false, Position = 0)]
        [Alias(aliasNames: "Message")]
        public string Text { get; set; }

        protected override void ProcessRecord()
        {
            if (!IsConditionScope())
            {
                throw new RuleRuntimeException(string.Format(PSRuleResources.KeywordConditionScope, LanguageKeywords.Recommend));
            }

            var result = GetResult();

            if (MyInvocation.BoundParameters.ContainsKey(nameof(Text)) && string.IsNullOrEmpty(result.Info.Recommendation))
            {
                result.Info.Recommendation = Text;
            }
        }
    }
}
