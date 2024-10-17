// Copyright © 2022-2024 Gabor Csizmadia
// This code is licensed under MIT license (see LICENSE for details)

using System.ComponentModel.DataAnnotations;

namespace Company.Product.ComponentA.NetCore
{
    public class ServiceAOptions
    {
        [StringLength(100)]
        public string WelcomeMessage { get; set; } = default!;
    }
}
