// ------------------------------------------------------------------------------------------------------
// <copyright file="AlgorandHelpers.cs" company="Nomis">
// Copyright (c) Nomis, 2023. All rights reserved.
// The Application under the MIT license. See LICENSE file in the solution root for full license information.
// </copyright>
// ------------------------------------------------------------------------------------------------------

using System.Numerics;

namespace Nomis.AlgoExplorer.Interfaces.Extensions
{
    /// <summary>
    /// Extension methods for Algorand.
    /// </summary>
    public static class AlgorandHelpers
    {
        /// <summary>
        /// Convert Wei value to ALGO.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total ALGO.</returns>
        public static decimal ToAlgo(this in BigInteger valueInWei)
        {
            if (valueInWei > new BigInteger(decimal.MaxValue))
            {
                return (decimal)(valueInWei / new BigInteger(100_000));
            }

            return (decimal)valueInWei * 0.000_001M;
        }

        /// <summary>
        /// Convert Wei value to ALGO.
        /// </summary>
        /// <param name="valueInWei">Wei.</param>
        /// <returns>Returns total ALGO.</returns>
        public static decimal ToAlgo(this decimal valueInWei)
        {
            return new BigInteger(valueInWei).ToAlgo();
        }
    }
}