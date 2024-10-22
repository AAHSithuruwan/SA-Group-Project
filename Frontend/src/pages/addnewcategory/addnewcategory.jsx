import React, { useState } from 'react';
import './addnewcategory.css';

const AddNewCategory = () => {
  const [image, setImage] = useState(null);
  const [description, setDescription] = useState('');

  const handleImageChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      setImage(URL.createObjectURL(file));
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
   
    console.log('Form Submitted');
  };

  return (
    <div className="add-category-page">
      <h2 className="add-category-page__title">Add New Category</h2>
      <form onSubmit={handleSubmit} className="add-category-page__form">
        <div className="add-category-page__input-row">
          <input type="text" id="categoryName" placeholder="Enter category name" required />
        </div>

        <div className="add-category-page__input-row add-category-page__file-row">
          <label htmlFor="categoryImage" className="add-category-page__file-label">Category Image</label>
          <input type="file" id="categoryImage" accept="image/*" onChange={handleImageChange} required />
          
          {image && (
            <div className="add-category-page__image-preview">
              <img src={image} alt="Selected" className="add-category-page__selected-image" />
            </div>
          )}
        </div>

        <div className="add-category-page__input-row">
        <textarea
                id="categoryDescription"
                value={description}
                onChange={(e) => setDescription(e.target.value)}
                placeholder="Enter category description"
                required
                className="add-category-page__textarea"
                ></textarea>
        </div>

        <button type="submit" className="add-category-page__submit-button">Submit</button>
      </form>
    </div>
  );
};

export default AddNewCategory;
