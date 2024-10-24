import React, { useState, useEffect } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './adminCategoryList.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';
import ConfirmDialogBox from '../../components/DialogBoxes/ConfirmDialogBox';

const adminCategoryList = () => {
  const [categories, setCategories] = useState([]);
  const navigate = useNavigate();

  const navigateTo = () => {
    navigate('/admincategorylist'); 
  };

  useEffect(() => {
    const fetchCategories = async () => {
      const jwtToken = await getJwtToken();

      try {
        const response = await axios.get(`http://localhost:5101/api/Category/All`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        });
        setCategories(response.data);
      } catch (error) {
        if (error.response && error.response.status === 401) {
          navigate('/signin');
        }
        console.error('There was an error fetching the categories!', error);
      }
    };

    fetchCategories();
  }, []);

  const handleUpdateClick = (categoryId) => {
    navigate('/admincategorylist/updatecategory', { state: { categoryId: categoryId } });
  };

  const handleNewCategoryClick = () => {
    navigate('/admincategorylist/addcategory');
  };

  const deleteCategoryDialog = (categoryId) => {
    ConfirmDialogBox({
      title: 'Are You Sure?',
      text: 'This action cannot be reverted!',
      onConfirm: () => deleteCategory(categoryId),
    })
  };

  const deleteCategory = async (categoryId) => {

    const jwtToken = await getJwtToken();

    try{
      const response = await axios.delete(`http://localhost:5101/api/Category/${categoryId}`, 
        {
          headers: {
              Authorization: `Bearer ${jwtToken}`,
          },
        }
    );

      if(response.status === 200){
        SuccessDialogBox({
          title: 'Category Deleted Successfully',
          text: 'The Category Has Been Deleted',
          onConfirm: navigateTo,
        })
      }
    }
    catch (error) {
      if(error.response && error.response.status === 401){
        navigate('/signin');
      }
      else{
        ErrorDialogBox({
          title: 'Category Deletion Failed',
          text: error.response.data
        })
      }
      console.error(error);
    }};

  return (
    <div className="category-container">
      <div className="category-details">
        <h2>CATEGORY DETAILS</h2>

        <div className="category-header">
          <button className="add-button" onClick={() => handleNewCategoryClick()}>Add New Category</button>
        </div>

        <table className="category-table">
          <thead>
            <tr>
              <th>Category</th>
              <th className="actions-column"></th>
              <th className='actions-column'></th>
            </tr>
          </thead>
          <tbody>
            {categories.length > 0 ? (
              categories.map((category) => (
                <tr key={category.categoryId}>
                  <td>
                    <div className="image-text-container">
                      <img
                        src={`http://localhost:5101/Images/CategoryImages/${category.categoryId}.png`}
                        alt={category.name}
                        className="category-image"
                      />
                      <span className="category-name">{category.name}</span>
                    </div>
                  </td>
                  <td>
                    <button className="update-button" onClick={() => handleUpdateClick(category.categoryId)}>Update</button>
                  </td>
                  <td>
                    <button className="remove-button" onClick={() => deleteCategoryDialog(category.categoryId)}>Remove</button>
                  </td>
                </tr>
              ))
            ) : (
              <tr>
                <td colSpan="2" className="no-categories-message">
                  <br />No categories available at the moment
                </td>
              </tr>
            )}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default adminCategoryList;
