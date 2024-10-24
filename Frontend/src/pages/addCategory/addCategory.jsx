import React, { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './addCategory.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const AddCategory = () => {
  const navigate = useNavigate();
  const [categoryName, setCategoryName] = useState('');
  const [categoryImage, setCategoryImage] = useState(null);

  const navigateTo = () => {
    navigate('/admincategorylist');
  };

  const handleSubmit = async (e) => {
    e.preventDefault();

    const jwtToken = await getJwtToken();

    const formData = new FormData();
    formData.append('Name', categoryName);
    formData.append('CategoryImage', categoryImage);

    try {
      const response = await axios.post('http://localhost:5101/api/Category', 
        formData,
        {
          headers: {
            'Content-Type': 'multipart/form-data',
            Authorization: `Bearer ${jwtToken}`,
          },
        }
      );

      if (response.status === 200) {
        SuccessDialogBox({
          title: 'Category Created Successfully',
          text: 'New Category Has Been Added',
          onConfirm: navigateTo,
        });
      }
    } catch (error) {
      if (error.response && error.response.status === 401) {
        navigate('/signin');
      } else {
        ErrorDialogBox({
          title: 'Category Creation Failed',
          text: error.response.data,
        });
      }
      console.error(error);
    }
  };

  return (
    <div className="add-category-container">
      <div className="category-form">
        <h2 className="form-title">ADD CATEGORY</h2>

        <form onSubmit={handleSubmit}>
          <div className="input-row">
            <label>Category Name:</label>
            <input type="text" value={categoryName} placeholder="Enter Category Name" onChange={(e) => setCategoryName(e.target.value)} required />
          </div>

          <div className="input-row">
            <label>Category Image:</label>
            <input type="file" onChange={(e) => setCategoryImage(e.target.files[0])} required />
          </div>

          <div className="submit-row">
            <button type="submit">Submit</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddCategory;
