// src/pages/categories/categories.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom'; 
import './categories.css'; 
import electronicsIcon from '../../assets/electronics.png';
import fashionIcon from '../../assets/fashion.png';
import homeGardenIcon from '../../assets/home-garden.png';
import sportsIcon from '../../assets/sports.png';
import vehiclesIcon from '../../assets/vehicles.png';
import jewelleryIcon from '../../assets/jewellery.png';
import antiquesIcon from '../../assets/antiques.png';
import artsIcon from '../../assets/arts.png';
import booksIcon from '../../assets/books.png';

const Categories = () => {
  const navigate = useNavigate(); 

  const categories = [
    { name: 'Electronics', image: electronicsIcon, bgColor: '#bfc9f9' },
    { name: 'Fashion', image: fashionIcon, bgColor: '#c5e8e3' },
    { name: 'Home & Garden', image: homeGardenIcon, bgColor: '#c5d7a3' },
    { name: 'Sports', image: sportsIcon, bgColor: '#f7e4a5' },
    { name: 'Vehicles', image: vehiclesIcon, bgColor: '#d4d3d1' },
    { name: 'Jewelleries', image: jewelleryIcon, bgColor: '#f3e3d3' },
    { name: 'Antiques', image: antiquesIcon, bgColor: '#b6c3c3' },
    { name: 'Arts', image: artsIcon, bgColor: '#e3b1b1' },
    { name: 'Books', image: booksIcon, bgColor: '#f9dd93' },
  ];

  const handleCategoryClick = (category) => {
    navigate('/categories/items', { state: { category: category.name } }); 
  };

  return (
    <section className="categories-section">
      <h2>Categories</h2>
      <div className="categories-grid">
        {categories.map((category, index) => (
          <div 
            key={index} 
            className="category-card" 
            style={{ backgroundColor: category.bgColor }}
            onClick={() => handleCategoryClick(category)} 
          >
            <img src={category.image} alt={category.name} />
            <p>{category.name}</p>
          </div>
        ))}
      </div>
    </section>
  );
};

export default Categories;
