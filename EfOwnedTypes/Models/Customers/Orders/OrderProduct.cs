﻿using System.Collections.Generic;
using System.Linq;
using EfOwnedTypes.Models.ForeignExchange;
using EfOwnedTypes.Models.Products;
using EfOwnedTypes.Models.SharedKernel;

namespace EfOwnedTypes.Models.Customers.Orders
{
    public class OrderProduct : Entity
    {
        public int Quantity { get; private set; }

        public ProductId ProductId { get; }

        internal MoneyValue Value { get; private set; }

        internal MoneyValue ValueInEUR { get; private set; }

        private OrderProduct()
        {
        }

        private OrderProduct(
            ProductPriceData productPrice,
            int quantity,
            string currency,
            List<ConversionRate> conversionRates)
        {
            ProductId = productPrice.ProductId;
            Quantity = quantity;

            CalculateValue(productPrice, currency, conversionRates);
        }

        internal static OrderProduct CreateForProduct(
            ProductPriceData productPrice, int quantity, string currency,
            List<ConversionRate> conversionRates)
        {
            return new OrderProduct(productPrice, quantity, currency, conversionRates);
        }

        internal void ChangeQuantity(ProductPriceData productPrice, int quantity, List<ConversionRate> conversionRates)
        {
            Quantity = quantity;

            CalculateValue(productPrice, Value.Currency, conversionRates);
        }

        private void CalculateValue(ProductPriceData productPrice, string currency,
            List<ConversionRate> conversionRates)
        {
            Value = Quantity * productPrice.Price;
            if (currency == "EUR")
            {
                ValueInEUR = Quantity * productPrice.Price;
            }
            else
            {
                var conversionRate =
                    conversionRates.Single(x => x.SourceCurrency == currency && x.TargetCurrency == "EUR");
                ValueInEUR = conversionRate.Convert(Value);
            }
        }
    }
}