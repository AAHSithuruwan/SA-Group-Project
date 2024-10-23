import React from 'react';
import { useNavigate } from 'react-router-dom';
import './categorydetails.css';
import pic1 from '../../assets/electronics.png';
import pic2 from '../../assets/fashion.png';
import pic3 from '../../assets/sports.png';

const categories = [
  {
    id: 1,
    image: pic1,
    name: 'Electronic',
  },
  {
    id: 2,
    image: pic2,
    name: 'Fashion',
  },
  {
    id: 3,
    image: pic3,
    name: 'Sports',
  },
];

const CategoryDetails = () => {
  const navigate = useNavigate();

  const handleAddCategory = () => {
    navigate('/addnewcategory');
  };

  return (
    <div className="category-container">
      <div className="category-details">
        <center><h2>Categories Details</h2></center>
        <button className="add-category-button" onClick={handleAddCategory}>Add new category</button>

        <table className="category-table">
          <thead>
            <tr>
              <th>Categories</th>
              <th></th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {categories.map((category) => (
              <tr key={category.id}>
                <td>
                  <img
                    src={category.image}
                    alt={category.name}
                    className="category-image"
                  />
                  <span>{category.name}</span>
                </td>
                <td>
                  <button className="edit-button">Edit</button>
                </td>
                <td>
                  <button className="remove-button">Remove</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default CategoryDetails;
