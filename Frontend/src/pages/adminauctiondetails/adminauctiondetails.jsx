import React from 'react'
import './adminauctiondetails.css';
import bag1 from '../../assets/bag.png';
import bag2 from '../../assets/bag.png';
import bag3 from '../../assets/bag.png';

const auctionItems = [
  {
    id: 1,
    image: bag1,
    productName: 'Céline - Classic - Shoulder bag',
    category: 'Fashion',
  },
  {
    id: 2,
    image: bag2,
    productName: 'Céline - Classic - Shoulder bag',
    category: 'Fashion',
  },
  {
    id: 3,
    image: bag3,
    productName: 'Céline - Classic - Shoulder bag',
    category: 'Fashion',
  },
];

const AdminAuctionDetails = () =>{
  return (
    
    
    <div className="auction-container">
      
      <div className="auction-details">
        <center><h2>Auction Details</h2></center>
        <table className="auction-table">
          <thead>
            <tr>
              <th>Product</th>
              <th>Status</th>
              <th></th>
            </tr>
          </thead>
          <tbody>
            {auctionItems.map((item) => (
              <tr key={item.id}>
                <td>
                  <img src={item.image} alt={item.productName} className="auction-image" />
                  <span>{item.productName}</span>
                </td>
                <td>{item.category}</td>
                <td>
                  <button className="details-button">Details</button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
};

export default  AdminAuctionDetails ;


