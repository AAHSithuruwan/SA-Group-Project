import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useLocation, useNavigate } from 'react-router-dom';
import './updateCategory.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const UpdateCategory = () => {
  const location = useLocation();
  const navigate = useNavigate();
  const [categoryName, setCategoryName] = useState('');
  const [categoryImage, setCategoryImage] = useState(null);
  const { categoryId } = location.state; // Get the categoryId from the location

  const navigateTo = () => {
    navigate('/admincategorylist');
  };

  useEffect(() => {
    const fetchCategory = async () => {
      const jwtToken = await getJwtToken();

      try {
        const response = await axios.get(`http://localhost:5101/api/Category/${categoryId}`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        setCategoryName(response.data.name);
      } catch (error) {
        if (error.response && error.response.status === 401) {
          navigate('/signin');
        } 
        console.error(error);
      }
    };

    fetchCategory();
  }, [categoryId, navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const jwtToken = await getJwtToken();

    const formData = new FormData();
    formData.append('categoryId', categoryId);
    formData.append('Name', categoryName);
    formData.append('CategoryImage', categoryImage);


    try {
      const response = await axios.put(`http://localhost:5101/api/Category`, 
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
          title: 'Category Updated Successfully',
          text: 'The Category Has Been Updated',
          onConfirm: navigateTo,
        });
      }
    } catch (error) {
      if (error.response && error.response.status === 401) {
        navigate('/signin');
      } else {
        ErrorDialogBox({
          title: 'Category Update Failed',
          text: error.response.data,
        });
      }
      console.error(error);
    }
  };

  return (
    <div className="update-category-container">
      <div className="category-form">
        <h2 className="form-title">UPDATE CATEGORY</h2>

        <form onSubmit={handleSubmit}>
          <div className="input-row">
            <label>Category Name:</label>
            <input type="text" value={categoryName} placeholder="Enter Category Name" onChange={(e) => setCategoryName(e.target.value)} required />
          </div>

          <div className="input-row">
            <label>Category Image:</label>
            <input type="file" onChange={(e) => setCategoryImage(e.target.files[0])} />
          </div>

          <div className="submit-row">
            <button type="submit">Update</button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default UpdateCategory;
