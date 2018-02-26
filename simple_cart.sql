-- phpMyAdmin SQL Dump
-- version 4.6.5.2
-- https://www.phpmyadmin.net/
--
-- Host: localhost:8889
-- Generation Time: Feb 26, 2018 at 11:49 PM
-- Server version: 5.6.35
-- PHP Version: 7.0.15

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `simple_cart`
--
CREATE DATABASE IF NOT EXISTS `simple_cart` DEFAULT CHARACTER SET utf8 COLLATE utf8_general_ci;
USE `simple_cart`;

-- --------------------------------------------------------

--
-- Table structure for table `cart_items`
--

CREATE TABLE `cart_items` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `item_id` int(11) DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `categories`
--

CREATE TABLE `categories` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `category_join`
--

CREATE TABLE `category_join` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `category_id` int(11) DEFAULT NULL,
  `item_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `items`
--

CREATE TABLE `items` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `name` varchar(255) DEFAULT NULL,
  `description` varchar(255) DEFAULT NULL,
  `cost` double DEFAULT NULL,
  `img` varchar(255) DEFAULT NULL,
  `stock` int(11) DEFAULT NULL,
  `category` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Dumping data for table `items`
--

INSERT INTO `items` (`id`, `name`, `description`, `cost`, `img`, `stock`, `category`) VALUES
(1, 'H&R Block Tax Software Deluxe + State 2017 with 5% Refund Bonus Offer [PC Download]', 'Note: You can access this item in Your Software Library. The number of computers eligible for installation may vary.\r\nBy placing your order, you agree to our Terms of Use.', 19.99, 'https://www.hrblock.com/images/schema-logo.jpg', 100, 'book'),
(2, 'A Wrinkle in Time (Time Quintet)', 'This is Book 1 of the Time Quintet Series\r\n\r\nIt was a dark and stormy night; Meg Murry, her small brother Charles Wallace, and her mother had come down to the kitchen for a midnight snack when they were upset by the arrival of a most disturbing stranger.', 4.66, 'https://lumiere-a.akamaihd.net/v1/images/p_awrinkleintime_7533a321.jpeg?region=0%2C0%2C300%2C450', 99, 'book'),
(3, 'Carhartt Womens Acrylic Watch Cap Medium Brown Heather Wool Beanie', 'A Great Hat', 30, 'https://images-na.ssl-images-amazon.com/images/I/51l3S9F5RJL._SY498_BO1,204,203,200_.jpg', 1000, 'clothing'),
(4, 'Carhartt Men\'s Signature Sleeve Logo Midweight Hooded Sweatshirt K288 (Dark Brown, XXX-Large)', 'Empty', 66.61, 'https://images-na.ssl-images-amazon.com/images/I/41mkCiht%2B5L._SY498_BO1,204,203,200_.jpg', 99, 'clothing'),
(5, 'Carhartt Men\'s Cmw6095 6\" Casual Wedge Work Boot', 'Leather\r\nImported\r\nRubber sole\r\nShaft measures approximately 7\" from arch\r\nSoft Toe ASTM 2892-11 EH', 111.8, 'https://images-na.ssl-images-amazon.com/images/I/81TV6nSDNDL._UX395_.jpg', 20, 'clothing'),
(6, 'Men Gucci Mane Ice Cream Tattoo Hoodies Sweatshirts Cool Hoodies Funny', '100% cotton\r\n100% Cotton Hooded Sweatshirt.\r\nFully Machine Washable.\r\nNo Kangaroo Pockets.\r\nExpected Shipping Time: 7-14 Working Days.', 200, 'https://media.gucci.com/style/DarkGray_Center_0_0_800x800/1475254832/454585_X5J57_9541_001_100_0000_Light-Cotton-sweatshirt-with-Gucci-logo.jpg', 1, 'clothing'),
(7, 'Apple Macbook Pro MJLQ2LL/A 15-inch Laptop (2.2 GHz Intel Core i7 Processor, 16GB RAM, 256 GB Hard Drive, Mac OS X)', '2.2GHz quad-core Intel Core i7\r\nTurbo Boost up to 3.4GHz\r\n16GB 1600MHz memory\r\n256GB PCIe-based flash storage\r\nIntel Iris Pro Graphics', 1800, 'https://crdms.images.consumerreports.org/prod/products/cr-legacy/production/products/testedmodel/profile/cr/jpg/598/381914-laptopcomputers-apple-macbookpro15inchwithretinadisplaymjlq2lla.jpg', 5, 'tools'),
(8, 'Pawkin Phthalate Free Cat Litter Mat, X-Large', 'PHTHALATE FREE - Independently laboratory tested non-toxic. Safe for your kitty and family.', 15.99, 'https://images-na.ssl-images-amazon.com/images/I/B1e0jpESVtS._SL1500_.jpg', 10, 'pet'),
(11, 'Osprey Men\'s Atmos 50 AG Backpacks', 'Synthetic\r\nPockets: 3 interior slip, 1 exterior', 158.35, 'https://images-na.ssl-images-amazon.com/images/I/81HPLAwxRGL._SL1500_.jpg', 99, 'outdoor'),
(12, 'Baby Wrap Carrier - Baby Sling by KeaBabies - 2 Colors - Baby Carrier Wrap - Baby Slings - Babys Wrap Carrier - Babies Wraps - Soft Ergonomic Stretchy Wrap', 'JUST THE RIGHT STRETCH - Our baby wrap carrier is specially designed using stretchy yet sturdy fabric so that baby’s weight will not be straining to your tired back & shoulders after long period of usage. ', 22.96, 'https://images-na.ssl-images-amazon.com/images/I/61BddfUKu9L._SL1500_.jpg', 3, 'outdoor');

-- --------------------------------------------------------

--
-- Table structure for table `orders`
--

CREATE TABLE `orders` (
  `id` bigint(20) UNSIGNED NOT NULL,
  `item_id` int(11) DEFAULT NULL,
  `user_id` int(11) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Table structure for table `users`
--

CREATE TABLE `users` (
  `id` int(11) NOT NULL,
  `login` varchar(255) NOT NULL,
  `password` varchar(255) NOT NULL,
  `name` varchar(255) NOT NULL,
  `email` varchar(255) NOT NULL,
  `address` int(11) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8 ROW_FORMAT=COMPACT;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `cart_items`
--
ALTER TABLE `cart_items`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `categories`
--
ALTER TABLE `categories`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `category_join`
--
ALTER TABLE `category_join`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `items`
--
ALTER TABLE `items`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `orders`
--
ALTER TABLE `orders`
  ADD PRIMARY KEY (`id`),
  ADD UNIQUE KEY `id` (`id`);

--
-- Indexes for table `users`
--
ALTER TABLE `users`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `cart_items`
--
ALTER TABLE `cart_items`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `categories`
--
ALTER TABLE `categories`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `category_join`
--
ALTER TABLE `category_join`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `items`
--
ALTER TABLE `items`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=13;
--
-- AUTO_INCREMENT for table `orders`
--
ALTER TABLE `orders`
  MODIFY `id` bigint(20) UNSIGNED NOT NULL AUTO_INCREMENT;
--
-- AUTO_INCREMENT for table `users`
--
ALTER TABLE `users`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
