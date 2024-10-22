// src/pages/categories/categories.jsx
import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom'; 
import './categories.css'; 

const Categories = () => {
  const navigate = useNavigate(); 

  const [categories, setCategories] = useState([]);

  useEffect(() => {
    const fetchCategories = async () => {
      try {
        const response = await axios.get('http://localhost:5101/api/Category/All');
        setCategories(response.data);
      } catch (error) {
        console.error('There was an error fetching the categories!', error);
      }
    };

    fetchCategories(); // Call the async function
  }, []);

  const handleCategoryClick = (category) => {
    navigate('/categories/items', { state: { categoryId: category.categoryId, categoryName: category.name } }); 
  };

  return (
    <section className="categories-section">
      <h2>Categories</h2>
      <div className="categories-grid">
        {categories.map((category, index) => (
          <div 
            key={index} 
            className="category-card" 
            onClick={() => handleCategoryClick(category)} 
          >
            <img src={`http://localhost:5101/Images/CategoryImages/${category.categoryId}.png`} alt={category.name} />
            <p>{category.name}</p>
            
          </div>
        ))}
      </div>
    </section>
  );
};

export default Categories;
