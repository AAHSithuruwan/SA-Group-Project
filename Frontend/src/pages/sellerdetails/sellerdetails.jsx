import React, { useEffect, useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';
import './sellerdetails.css';
import ErrorDialogBox from '../../components/DialogBoxes/ErrorDialogBox';
import SuccessDialogBox from '../../components/DialogBoxes/SuccessDialogBox';
import { getJwtToken } from '../../components/JwtAuthentication/JwtTokenHandler';

const SellerDetails = () => {

  const navigate = useNavigate();
  const [firstName, setFirstName] = useState([]);
  const [lastName, setLastName] = useState([]);
  const [email, setEmail] = useState([]);
  const [phoneNumber, setPhoneNumber] = useState([]);
  const [address, setAddress] = useState([]);
  const [sellerImage, setSellerImage] = useState([]);

  const refresh = () => {
    navigate('/sellerdetails');
  }

  useEffect(() => {
    const fetchSellerDetails = async () => {
      const jwtToken = await getJwtToken();
      console.log(jwtToken);
      try {
        const response = await axios.get('http://localhost:5101/api/Seller',{
          headers: {
              Authorization: `Bearer ${jwtToken}`,
          },
        });
        setFirstName(response.data.firstName);
        setLastName(response.data.lastName);
        setEmail(response.data.email);
        setPhoneNumber(response.data.phoneNumber);
        setAddress(response.data.address);
      } catch (error) {
        if(error.response && error.response.status === 401){
          navigate('/signin');
        }
        console.error(error.response);
      }
    };

    fetchSellerDetails(); // Call the async function
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const jwtToken = await getJwtToken();

    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if(!(emailRegex.test(email))){
      ErrorDialogBox({
        title: 'Information Update Failed!',
        text: 'Please enter a valid email address'
      });
      return;
    }

    const formData = new FormData();
    formData.append('FirstName', firstName);
    formData.append('LastName', lastName);
    formData.append('Email', email);
    formData.append('PhoneNumber', phoneNumber);
    formData.append('Address', address);
    formData.append('SellerImage', sellerImage);

    try{
      const response = await axios.put('http://localhost:5101/api/Seller', 
        formData,
        {
          headers: {
              'Content-Type': 'multipart/form-data',
              Authorization: `Bearer ${jwtToken}`,
          },
        }
    );

      if(response.status === 200){
        SuccessDialogBox({
          title: 'Information Updated Successfully',
          text: 'Details updated successfully',
          onConfirm: refresh,
        })
      }
    }
    catch (error) {
      if(error.response && error.response.status === 401){
        navigate('/signin');
      }
      else{
        ErrorDialogBox({
          title: 'Information Update Failed',
          text: error.response.data
        })
      }
      console.error(error);
    }};

  return (
    <div className="details-container">
      <div className="details-form">
        <h2 className="form-title">SELLER INFORMATION</h2>
      <form onSubmit={handleSubmit}>
        <div className="input-row">
          <label>First Name:</label>
          <input type="text" value={firstName} onChange={(e) => setFirstName(e.target.value)} required/>
        </div>

        <div className="input-row">
          <label>Last Name:</label>
          <input type="text" value={lastName} onChange={(e) => setLastName(e.target.value)} required />
        </div>

        <div className="input-row">
          <label>Email:</label>
          <input type="text" value={email} onChange={(e) => setEmail(e.target.value)} required />
        </div>

        <div className="input-row">
          <label>Contact Number:</label>
          <input type="tel" value={phoneNumber} onChange={(e) => setPhoneNumber(e.target.value)} required />
        </div>

        <div className="input-row">
          <label>Address:</label>
          <input type="text" value={address} onChange={(e) => setAddress(e.target.value)} required />
        </div>

        <div className="input-row">
          <label>Add Profile Image:</label>
          <input type="file" onChange={(e) => setSellerImage(e.target.files[0])} />
        </div>

        <div className="submit-row">
          <button type="submit">Submit</button>
        </div>
      </form>
      </div>
    </div>
  );
};

export default SellerDetails;
