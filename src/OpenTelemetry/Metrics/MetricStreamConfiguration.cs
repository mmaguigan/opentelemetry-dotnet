// Copyright The OpenTelemetry Authors
// SPDX-License-Identifier: Apache-2.0

namespace OpenTelemetry.Metrics;

/// <summary>
/// Stores configuration for a MetricStream.
/// </summary>
public class MetricStreamConfiguration
{
    private string? name;

    /// <summary>
    /// Gets the drop configuration.
    /// </summary>
    /// <remarks>
    /// Note: All metrics for the given instrument will be dropped (not
    /// collected).
    /// </remarks>
    public static MetricStreamConfiguration Drop { get; } = new MetricStreamConfiguration { ViewId = -1 };

    /// <summary>
    /// Gets or sets the optional name of the metric stream.
    /// </summary>
    /// <remarks>
    /// Note: If not provided the instrument name will be used.
    /// </remarks>
    public string? Name
    {
        get => this.name;
        set
        {
            if (value != null && !MeterProviderBuilderSdk.IsValidViewName(value))
            {
                throw new ArgumentException($"Custom view name {value} is invalid.", nameof(value));
            }

            this.name = value;
        }
    }

    /// <summary>
    /// Gets or sets the optional description of the metric stream.
    /// </summary>
    /// <remarks>
    /// Note: If not provided the instrument description will be used.
    /// </remarks>
    public string? Description { get; set; }

    /// <summary>
    /// Gets or sets the optional tag keys to include in the metric stream.
    /// </summary>
    /// <remarks>
    /// Notes:
    /// <list type="bullet">
    /// <item>If not provided, all the tags provided by the instrument
    /// while reporting measurements will be used for aggregation.
    /// If provided, only those tags in this list will be used
    /// for aggregation. Providing an empty array will result
    /// in a metric stream without any tags.
    /// </item>
    /// <item>A copy is made of the provided array.</item>
    /// </list>
    /// </remarks>
    public string[]? TagKeys
    {
        get
        {
            if (this.CopiedTagKeys != null)
            {
                string[] copy = new string[this.CopiedTagKeys.Length];
                this.CopiedTagKeys.AsSpan().CopyTo(copy);
                return copy;
            }

            return null;
        }

        set
        {
            if (value != null)
            {
                string[] copy = new string[value.Length];
                value.AsSpan().CopyTo(copy);
                this.CopiedTagKeys = copy;
            }
            else
            {
                this.CopiedTagKeys = null;
            }
        }
    }

    internal string[]? CopiedTagKeys { get; private set; }

    internal int? ViewId { get; set; }

    // TODO: MetricPoints caps can be configured here on
    // a per stream basis, when we add such a capability
    // in the future.
}
