// src/pages/categoryItems/categoryItems.jsx
import React from 'react';
import { useLocation, useNavigate } from 'react-router-dom';
import './categoryItems.css';
import bag from '../../assets/electronics1.png'; 

const itemsData = {
  Electronics: [
    { name: 'Smartphone', price: '$700', image: 'path-to-smartphone-image' },
    { name: 'Laptop', price: '$1,200', image: 'path-to-laptop-image' },
    { name: 'Headphones', price: '$150', image: 'path-to-headphones-image' },
    { name: 'Smartwatch', price: '$250', image: 'path-to-smartwatch-image' },
    { name: 'Camera', price: '$500', image: 'path-to-camera-image' },
    { name: 'Tablet', price: '$300', image: 'path-to-tablet-image' },
  ],
  Fashion: [
    { name: 'T-Shirt', price: '$25', image: 'path-to-tshirt-image' },
    { name: 'Jeans', price: '$40', image: 'path-to-jeans-image' },
    { name: 'Jacket', price: '$100', image: 'path-to-jacket-image' },
    { name: 'Dress', price: '$80', image: 'path-to-dress-image' },
    { name: 'Shoes', price: '$60', image: 'path-to-shoes-image' },
    { name: 'Hat', price: '$20', image: 'path-to-hat-image' },
  ],
  'Home & Garden': [
    { name: 'Sofa', price: '$500', image: 'path-to-sofa-image' },
    { name: 'Dining Table', price: '$300', image: 'path-to-dining-table-image' },
    { name: 'Bed', price: '$400', image: 'path-to-bed-image' },
    { name: 'Garden Chair', price: '$80', image: 'path-to-garden-chair-image' },
    { name: 'Lamp', price: '$40', image: 'path-to-lamp-image' },
    { name: 'Rug', price: '$150', image: 'path-to-rug-image' },
  ],
  Sports: [
    { name: 'Basketball', price: '$25', image: 'path-to-basketball-image' },
    { name: 'Tennis Racket', price: '$60', image: 'path-to-tennis-racket-image' },
    { name: 'Soccer Ball', price: '$30', image: 'path-to-soccer-ball-image' },
    { name: 'Yoga Mat', price: '$20', image: 'path-to-yoga-mat-image' },
    { name: 'Dumbbells', price: '$50', image: 'path-to-dumbbells-image' },
    { name: 'Bicycle', price: '$300', image: 'path-to-bicycle-image' },
  ],
  Vehicles: [
    { name: 'Car', price: '$20,000', image: 'path-to-car-image' },
    { name: 'Motorbike', price: '$10,000', image: 'path-to-motorbike-image' },
    { name: 'Bicycle', price: '$500', image: 'path-to-bicycle-image' },
    { name: 'Truck', price: '$30,000', image: 'path-to-truck-image' },
    { name: 'Van', price: '$25,000', image: 'path-to-van-image' },
    { name: 'Scooter', price: '$3,000', image: 'path-to-scooter-image' },
  ],
  Jewelleries: [
    { name: 'Necklace', price: '$150', image: 'path-to-necklace-image' },
    { name: 'Ring', price: '$100', image: 'path-to-ring-image' },
    { name: 'Earrings', price: '$75', image: 'path-to-earrings-image' },
    { name: 'Bracelet', price: '$50', image: 'path-to-bracelet-image' },
    { name: 'Watch', price: '$200', image: 'path-to-watch-image' },
    { name: 'Brooch', price: '$80', image: 'path-to-brooch-image' },
  ],
  Antiques: [
    { name: 'Antique Vase', price: '$500', image: 'path-to-antique-vase-image' },
    { name: 'Old Clock', price: '$300', image: 'path-to-old-clock-image' },
    { name: 'Classic Painting', price: '$1,000', image: 'path-to-classic-painting-image' },
    { name: 'Old Book', price: '$50', image: 'path-to-old-book-image' },
    { name: 'Vintage Table', price: '$600', image: 'path-to-vintage-table-image' },
    { name: 'Antique Chair', price: '$400', image: 'path-to-antique-chair-image' },
  ],
  Arts: [
    { name: 'Painting', price: '$200', image: 'path-to-painting-image' },
    { name: 'Sculpture', price: '$300', image: 'path-to-sculpture-image' },
    { name: 'Drawing', price: '$100', image: 'path-to-drawing-image' },
    { name: 'Photography', price: '$150', image: 'path-to-photography-image' },
    { name: 'Craft', price: '$50', image: 'path-to-craft-image' },
    { name: 'Pottery', price: '$80', image: 'path-to-pottery-image' },
  ],
  Books: [
    { name: 'Fiction Book', price: '$15', image: 'path-to-fiction-book-image' },
    { name: 'Non-Fiction Book', price: '$20', image: 'path-to-non-fiction-book-image' },
    { name: 'Children\'s Book', price: '$10', image: 'path-to-childrens-book-image' },
    { name: 'Textbook', price: '$50', image: 'path-to-textbook-image' },
    { name: 'Cookbook', price: '$30', image: 'path-to-cookbook-image' },
    { name: 'Biography', price: '$25', image: 'path-to-biography-image' },
  ],
};


itemsData.Electronics[0].image = bag; 
itemsData.Fashion[0].image = bag; 
itemsData['Home & Garden'][0].image = bag; 
itemsData.Sports[0].image = bag; 
itemsData.Vehicles[0].image = bag; 
itemsData.Jewelleries[0].image = bag; 
itemsData. Antiques[0].image = bag; 
itemsData.Arts[0].image = bag; 
itemsData.Books[0].image = bag; 

const CategoryItems = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const categoryName = location.state.category;
  const items = itemsData[categoryName] || [];

  const handleItemClick = (item) => {
    // Navigate to the item details page and pass the item data
    navigate('/item-details', { state: { item } });
  };

  return (
    <section className="category-items-section">
      <h2>{categoryName} Items</h2>
      <div className="items-grid">
        {items.map((item, index) => (
          <div key={index} className="item-card" onClick={() => handleItemClick(item)}>
            <img src={item.image} alt={item.name} />
            <h3>{item.name}</h3>
            <p>{item.price}</p>
          </div>
        ))}
      </div>
    </section>
  );
};

export default CategoryItems;
