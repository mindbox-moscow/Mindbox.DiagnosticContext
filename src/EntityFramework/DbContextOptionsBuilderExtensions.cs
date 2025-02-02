// Copyright 2021 Mindbox Ltd
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Mindbox.DiagnosticContext.EntityFramework;

public static class EntityFrameworkDiagnosticContextExtensions
{
	public static DbContextOptionsBuilder AddEfCommandsMetrics(
		this DbContextOptionsBuilder serviceCollection,
		IEnumerable<IEfCommandMetricsCounter>? metricsCounters = null)
	{
		var counters = metricsCounters?.ToList() ?? new List<IEfCommandMetricsCounter>();

		if (counters.All(counter => counter is not EfCommandsMetrics))
			counters.Add(EfCommandsMetrics.Instance);

		return serviceCollection.AddInterceptors(new EfCommandsScorerInterceptor(counters));
	}
}